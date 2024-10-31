using Cosmos.Core;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using WinttOS.Core;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Kernel;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Filesystem;
using WinttOS.wSystem.IO;
using Sys = Cosmos.System;

namespace WinttOS
{
    public class Kernel : Sys.Kernel
    {
        #region Fields

        public const string KernelVersion = "WinttCore v0.2.0";
        public static StringCollection ReadonlyFiles { get; internal set; } = new();
        public static StringCollection ReadonlyDirectories { get; internal set; } = new();
        public static bool IsFinishingKernel { get; private set; } = false;
        public static bool IsRebooting { get; private set; } = false;
        public static string CpuBrandName { get; private set; } = "Unknown";
        public static long CpuClockSpeed { get; private set; } = -1;
        public static string CpuVendorName { get; private set; } = "Unknown";

        public static readonly List<Action> OnKernelFinish = new();
        private static Kernel _instance = null;

        #endregion

        #region Methods
        protected override void BeforeRun()
        {
            bool hasError = false;
            try
            {
                ShellUtils.PrintTaskResult("Collecting", ShellTaskResult.DOING, "Hardware info");
                ShellUtils.MoveCursorUp();
                Logger.DoBootLog("[Info] Collecting CPU info");
                try
                {
                    if (CPU.CanReadCPUID() != 0)
                    {
                        CpuBrandName = CPU.GetCPUBrandString();
                        CpuClockSpeed = CPU.GetCPUCycleSpeed();
                        CpuVendorName = CPU.GetCPUVendorName();
                        Logger.DoBootLog("[OK] CPU info collected");
                        ShellUtils.PrintTaskResult("Collecting", ShellTaskResult.OK, "Hardware info");
                    }
                    else
                    {
                        hasError = true;
                        Logger.DoBootLog("[Error] Cannot collect CPU info -> Unable to read CPUID");
                        ShellUtils.PrintTaskResult("Collecting", ShellTaskResult.FAILED, "Hardware info");

                    }
                }
                catch (Exception e)
                {
                    hasError = true;
                    Logger.DoBootLog("[Error] Cannot collect CPU info -> " + e.Message);
                    ShellUtils.PrintTaskResult("Collecting", ShellTaskResult.FAILED, "Hardware info");

                }

                ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.DOING, "VFS Filesystem");

                Logger.DoBootLog("[Info] Registering filesystem");

                GlobalData.FileSystem = new CosmosVFS();
                VFSManager.RegisterVFS(GlobalData.FileSystem);

                ShellUtils.MoveCursorUp();
                ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.OK, "VFS Filesystem");

                Logger.DoBootLog("[Info] Starting to mount disks");
                ShellUtils.PrintTaskResult("Mounting disks", ShellTaskResult.NONE);

                GlobalData.FileSystem.Initialize(true);

                Logger.DoBootLog("[Info] Loading system resources");

                BootResourceLoader.LoadResources();

                wSystem.WinttOS.InitializeSystem();
                

            } catch (Exception ex)
            {
                WinttRaiseHardError(ex.Message, this);
            }
        }

        protected override void Run()
        { }

        public static void SendDbg(string msg) => _instance.mDebugger.Send(msg);

        protected override void AfterRun()
        {
            Console.WriteLine("It is now safe to turn off your computer!");
            Sys.Power.Shutdown();
        }


        public static void ShutdownKernel()
        {
            try
            {
                if (FullScreenCanvas.IsInUse)
                {
                    FullScreenCanvas.Disable();

                    wSystem.WinttOS.IsTty = false;

                    SystemIO.STDOUT = new ConsoleIO();
                    SystemIO.STDERR = new ConsoleIO();
                    SystemIO.STDIN = new ConsoleIO();

                    Console.Clear();
                }
                
                wSystem.WinttOS.KernelPrint = true;

                ShellUtils.PrintTaskResult("Shutting down", ShellTaskResult.NONE);

                ShellUtils.PrintTaskResult("Running", ShellTaskResult.DOING, "Shutdown actions");
                foreach (var action in OnKernelFinish)
                {
                    action();
                }
                ShellUtils.MoveCursorUp();
                ShellUtils.PrintTaskResult("Running", ShellTaskResult.OK, "Shutdown actions");

                IsFinishingKernel = true;

                ShellUtils.PrintTaskResult("Finishing", ShellTaskResult.DOING, "Tasks");

            }
            catch (Exception ex)
            {
                WinttRaiseHardError(ex.Message, _instance);
            }
        }

        public static void RebootKernel()
        {
            IsRebooting = true;
            ShutdownKernel();
        }

        #endregion

        #region API

        public static void WinttRaiseHardError(string message, object sender)
        {
            
            _ = new KernelPanic(message, sender);
        }

        public static void WinttRaiseHardError(string message, HALException exception)
        {

            _ = new KernelPanic(message, exception);
        }

        #endregion
    }
}

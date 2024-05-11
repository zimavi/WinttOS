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
using WinttOS.wSystem.IO;
using Sys = Cosmos.System;

namespace WinttOS
{
    public class Kernel : Sys.Kernel
    {
        #region Fields

        public const string KernelVersion = "WinttCore v0.1.0";
        public static StringCollection ReadonlyFiles { get; internal set; } = new();
        public static StringCollection ReadonlyDirectories { get; internal set; } = new();
        public static bool IsFinishingKernel { get; private set; } = false;
        public static bool IsRebooting { get; private set; } = false;

        public static readonly List<Action> OnKernelFinish = new();
        private static Kernel _instance = null;

        #endregion

        #region Methods
        protected override void BeforeRun()
        {
            try
            {
                ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.DOING, "VFS Filesystem");

                Logger.DoBootLog("[Info] Registering filesystem");

                GlobalData.FileSystem = new CosmosVFS();
                VFSManager.RegisterVFS(GlobalData.FileSystem);
                GlobalData.FileSystem.Initialize(true);

                ShellUtils.MoveCursorUp();
                ShellUtils.PrintTaskResult("Initializing", ShellTaskResult.OK, "VFS Filesystem");

                Logger.DoBootLog("[Info] Mounting avaiable disks");

                List<Disk> disks = VFSManager.GetDisks();

                for (int i = 0; i < disks.Count; i++)
                {
                    ShellUtils.PrintTaskResult("Mounting", ShellTaskResult.DOING, "Disk" + i);
                    ShellUtils.MoveCursorUp();
                    try
                    {
                        disks[i].Mount();
                        ShellUtils.PrintTaskResult("Mounting", ShellTaskResult.OK, "Disk" + i);
                        Logger.DoBootLog("[Info] Disk #" + i + " mounted");
                    }
                    catch
                    {
                        ShellUtils.PrintTaskResult("Mounting", ShellTaskResult.FAILED, "Disk" + i);
                        Logger.DoBootLog("[Info] Unable to mount disk #" + i + "!");
                    }
                }

                Logger.DoBootLog("[Info] Loading system resources");

                ShellUtils.PrintTaskResult("Loading", ShellTaskResult.DOING, "System Fonts");
                
                ShellUtils.MoveCursorUp();

                bool found = false;

                foreach(Disk disk in disks)
                {
                    foreach(var part in disk.Partitions)
                    {
                        if (part.HasFileSystem)
                        {
                            Logger.DoBootLog("[Info] Searching for 'zap-ext-light18.psf'");

                            if (File.Exists(part.RootPath + @"resources\zap-ext-light18.psf"))
                            {
                                try
                                {
                                    Logger.DoBootLog("[OK] Found 'zap-ext-light18.psf'");
                                    Logger.DoBootLog("[Info] Loading resource");

                                    byte[] font = File.ReadAllBytes(part.RootPath + @"resources\zap-ext-light18.psf");
                                    Files.Fonts.Font18 = PCScreenFont.LoadFont(font);
                                    found = true;
                                    ShellUtils.PrintTaskResult("Loading", ShellTaskResult.OK, "System Fonts");

                                    Logger.DoBootLog("[OK] 'zap-ext-light18.psf' loaded");
                                    break;
                                } 
                                catch (ArgumentException)
                                {
                                    ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "System Fonts: Invalid file");
                                    _ = new KernelPanic("Kernel load failed: Invalid resource file", this);
                                }
                                catch (Exception e)
                                {
                                    ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "System Fonts: " + e.Message);
                                    _ = new KernelPanic("Kernel load failed: Invalid resource file", this);
                                }
                            }
                        }
                    }
                    if (found)
                        break;
                }

                if(!found)
                {
                    ShellUtils.PrintTaskResult("Loading", ShellTaskResult.FAILED, "System Fonts: Not found");
                    _ = new KernelPanic("Kernel load failed: Cannot find resource file", this);
                }

                Console.Write("Press any key to continue boot...");
                Console.ReadKey();

                wSystem.WinttOS.InitializeSystem();
                

            } catch (Exception ex)
            {
                WinttRaiseHardError(ex.Message, this);
            }
        }

        protected override void Run()
        { }

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

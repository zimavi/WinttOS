﻿using System;
using WinttOS.Core;
using Sys = Cosmos.System;
using WinttOS.Core.Utils.Debugging;
using System.Collections.Specialized;
using System.Collections.Generic;
using WinttOS.Core.Utils.System;

namespace WinttOS
{
    public class Kernel : Sys.Kernel
    {
        #region Fields

        public const string KernelVersion = "WinttCore v0.1.0";
        public static StringCollection ReadonlyFiles { get; internal set; }
        public static StringCollection ReadonlyDirectories { get; internal set; }
        public static bool IsFinishingKernel { get; private set; } = false;
        public static bool IsRebooting { get; private set; } = false;

        public static readonly List<Action> OnKernelFinish = new();

        #endregion

        #region Methods
        protected override void BeforeRun()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Kernel.BeforeRun()","void()","Kernel.cs",32));
            try
            {
                WinttDebugger.Trace("Registering readonly files", this);
                ReadonlyFiles = new()
                {
                    @"0:\Root.txt",
                    @"0:\Kudzu.txt",
                };

                WinttDebugger.Trace("Registering readonly directories", this);
                ReadonlyDirectories = new()
                {
                    @"0:\WinttOS",
                    @"0:\WinttOS\System32"
                };
                

                ShellUtils.MoveCursorUp(-1);

                ShellUtils.PrintTaskResult("Registration File System", ShellTaskResult.DOING);

                WinttDebugger.Debug("Registering filesystem", this);
                GlobalData.FileSystem = new Sys.FileSystem.CosmosVFS();
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(GlobalData.FileSystem);
                GlobalData.FileSystem.Initialize(true);

                ShellUtils.MoveCursorUp();
                ShellUtils.PrintTaskResult("Registration File System", ShellTaskResult.OK);

                WinttDebugger.Trace("Kernel initialize complete! Coming to system");
                System.WinttOS.InitializeSystem();
                

            } catch (Exception ex)
            {
                WinttDebugger.Critical(ex.Message, true, this);
            }
            finally
            {
                WinttCallStack.RegisterReturn();
            }
        }

        protected override void Run()
        { }

        protected override void AfterRun()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Kernel.AfterRun", "void()", "Kernel.cs", 81));
            Console.WriteLine("It is now safe to turn off your computer!");
            Sys.Power.Shutdown();
            WinttCallStack.RegisterReturn();
        }


        public static void ShutdownKernel()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Kernel.ShutdownKernel()", "void()", "Kernel.cs", 87));
            try
            {
                foreach (var action in OnKernelFinish)
                {
                    action();
                }
                Console.Clear();
                if (!IsRebooting)
                    Console.WriteLine("Shutting down...");
                else
                    Console.WriteLine("Rebooting...");
                IsFinishingKernel = true;
            }
            catch (Exception ex)
            {
                WinttDebugger.Critical("Something happened on shutdown!", ex);
            }
            finally
            {
                WinttCallStack.RegisterReturn();
            }
        }

        public static void RebootKernel()
        {
            WinttCallStack.RegisterCall(new("WinttOS.Kernel.RebootKernel()", "void()", "Kernel.cs", 117));
            IsRebooting = true;
            ShutdownKernel();
            WinttCallStack.RegisterReturn();
        }

        #endregion
    }
}

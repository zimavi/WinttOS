using Cosmos.Core.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Kernel;
using WinttOS.Core.Utils.Sys;

namespace WinttOS.Core.Utils.Debugging
{
    /// <summary>
    /// WinttOS to COM debugger
    /// </summary>
    public class WinttDebugger
    {
        public static List<string> ErrorMessages { get;} = new(); 

        /// <summary>
        /// Send trace log to COM debugger
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sender">Object that sends message</param>
        public static void Trace(string message, object sender = null)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Core.Utils.Debugging.WinttDebugger.Trace()",
                "void(string, object)", "WinttDebugger.cs", 22));
            if (sender != null)
            {
                Cosmos.System.Global.Debugger.Send($"[Trace] From {sender.GetType().Name}: {message}");
                ErrorMessages.Add($"[Trace] From {sender.GetType().Name}: {message}");
            }
            else
            {
                Cosmos.System.Global.Debugger.Send($"[Trace] {message}");
                ErrorMessages.Add($"[Trace] {message}");
            }
            WinttCallStack.RegisterReturn();
        }
        /// <summary>
        /// Send debug log to COM debugger
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sender">Object that sends message</param>
        public static void Debug(string message, object sender = null)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Core.Utils.Debugging.WinttDebugger.Debug()",
                "void(string, object)", "WinttDebugger.cs", 37));
            if (sender != null)
            {
                Cosmos.System.Global.Debugger.Send($"[Debug] From {sender.GetType().Name}: {message}");
                ErrorMessages.Add($"[Debug] From {sender.GetType().Name}: {message}");
            }
            else
            {
                Cosmos.System.Global.Debugger.Send($"[Debug] {message}");
                ErrorMessages.Add($"[Debug] {message}");
            }
            WinttCallStack.RegisterReturn();
        }
        /// <summary>
        /// Send log to COM debugger
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sender">Object that sends message</param>
        public static void Info(string message, object sender = null)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Core.Utils.Debugging.WinttDebugger.Info()",
                "void(string, object)", "WinttDebugger.cs", 52));
            if (sender != null)
            {
                Cosmos.System.Global.Debugger.Send($"[Info] From {sender.GetType().Name}: {message}");
                ErrorMessages.Add($"[Info] From {sender.GetType().Name}: {message}");
            }
            else
            {
                Cosmos.System.Global.Debugger.Send($"[Info] {message}");
                ErrorMessages.Add($"[Info] {message}");
            }
            WinttCallStack.RegisterReturn();
        }
        /// <summary>
        /// Send warn log to COM debugger
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sender">Object that sends message</param>
        public static void Warning(string message, object sender = null)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Core.Utils.Debugging.WinttDebugger.Warning()",
                "void(string, object)", "WinttDebugger.cs", 67));
            if (sender != null)
            {
                Cosmos.System.Global.Debugger.Send($"[Warn] From {sender.GetType().Name}: {message}");
                ErrorMessages.Add($"[Warn] From {sender.GetType().Name}: {message}");
            }
            else
            {
                Cosmos.System.Global.Debugger.Send($"[Warn] {message}");
                ErrorMessages.Add($"[Warn] {message}");
            }
            WinttCallStack.RegisterReturn();
        }
        /// <summary>
        /// Send error log to COM debugger
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sendMsgBox">Should debugger get message box</param>
        /// <param name="sender">Object that sends message</param>
        public static void Error(string message, bool sendMsgBox, object sender = null)
        {
            WinttCallStack.RegisterCall(new("WinttOS.Core.Utils.Debugging.WinttDebugger.Error()",
                "void(string, bool, object)", "WinttDebugger.cs", 83));
            if (sender != null)
            {
                Cosmos.System.Global.Debugger.Send($"[Error] From {sender.GetType().Name}: {message}");
                ErrorMessages.Add($"[Error] From {sender.GetType().Name}: {message}");
                if (sendMsgBox)
                    Cosmos.System.Global.Debugger.SendMessageBox($"Got error from {sender.GetType().ToString()}: {message}");
            }
            else
            {
                Cosmos.System.Global.Debugger.Send($"[Error] {message}");
                ErrorMessages.Add($"[Error] {message}");
                if (sendMsgBox)
                    Cosmos.System.Global.Debugger.SendMessageBox($"Got error: {message}");
            }
            WinttCallStack.RegisterReturn();
        }
        /// <summary>
        /// Send serve/critical log to COM debugger
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="executePanic">Should fall in kernel panic after sending log</param>
        /// <param name="sender">Object that sends message</param>
        public static void Critical(string message, bool executePanic = true, object sender = null)
        {
            //if (sender != null)
            //{
            //    Cosmos.System.Global.Debugger.Send($"[Serve] From {sender.GetType()}: {message}");
            //    if(!executePanic)
            //        Cosmos.System.Global.Debugger.SendMessageBox($"Got fatal error from {sender.GetType()}: {message}");
            //    else
            //        Cosmos.System.Global.Debugger.SendMessageBox($"Got fatal error from {sender.GetType()}: {message}\n{WinttCallStack.GetCallStack()}");
            //}
            //else
            //{
                Cosmos.System.Global.Debugger.Send($"[Serve] {message}");
                ErrorMessages.Add($"[Serve] {message}\n\n{WinttCallStack.GetCallStack()}");
                if(!executePanic)
                    Cosmos.System.Global.Debugger.SendMessageBox($"Got fatal error: {message}");
                else
                    Cosmos.System.Global.Debugger.SendMessageBox($"Got fatal error: {message}\n\n{WinttCallStack.GetCallStack()}");
            //}
            if (executePanic)
            {
                //if (sender != null)
                //    _ = new KernelPanic(message, sender);
                //else
                //    _ = new KernelPanic(message, "Unknown source");
            }
            WinttCallStack.RegisterReturn();
        }
        public static void Critical(string message, Exception exception, object sender = null)
        {
            //if (!sender.IsNull())
            //{
            //    Cosmos.System.Global.Debugger.Send($"[Serve] From {sender.GetType()}: {message}");
            //    Cosmos.System.Global.Debugger.SendMessageBox($"Got fatal error from {sender.GetType()}: {message}\n{exception.Message}\n{WinttCallStack.GetCallStack()}");
            //}
            //else
            //{
                Cosmos.System.Global.Debugger.Send($"[Serve] {message}");
                Cosmos.System.Global.Debugger.SendMessageBox($"Got fatal error: {message}\n{exception.Message}\n{WinttCallStack.GetCallStack()}");
            //}
            //if (!sender.IsNull())
            //    _ = new KernelPanic(message, sender, exception);
            //else
            //    _ = new KernelPanic(message, "Unknown source");
        }
    }
}

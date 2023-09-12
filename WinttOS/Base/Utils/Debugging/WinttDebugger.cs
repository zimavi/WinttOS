using Cosmos.Core.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Kernel;

namespace WinttOS.Base.Utils.Debugging
{
    /// <summary>
    /// WinttOS to COM debugger
    /// </summary>
    public class WinttDebugger
    {
        /// <summary>
        /// Send trace log to COM debugger
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sender">Object that sends message</param>
        public static void Trace(string message, object sender = null)
        {
            if (sender != null)
                Cosmos.System.Global.Debugger.Send($"[Trace] From {sender.GetType().Name}: {message}");
            else
                Cosmos.System.Global.Debugger.Send($"[Trace] {message}");
        }
        /// <summary>
        /// Send debug log to COM debugger
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sender">Object that sends message</param>
        public static void Debug(string message, object sender = null)
        {
            if (sender != null)
                Cosmos.System.Global.Debugger.Send($"[Debug] From {sender.GetType().Name}: {message}");
            else
                Cosmos.System.Global.Debugger.Send($"[Debug] {message}");
        }
        /// <summary>
        /// Send log to COM debugger
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sender">Object that sends message</param>
        public static void Info(string message, object sender = null)
        {
            if (sender != null)
                Cosmos.System.Global.Debugger.Send($"[Info] From {sender.GetType().Name}: {message}");
            else
                Cosmos.System.Global.Debugger.Send($"[Info] {message}");
        }
        /// <summary>
        /// Send warn log to COM debugger
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sender">Object that sends message</param>
        public static void Warning(string message, object sender = null)
        {
            if (sender != null)
                Cosmos.System.Global.Debugger.Send($"[Warn] From {sender.GetType().Name}: {message}");
            else
                Cosmos.System.Global.Debugger.Send($"[Warn] {message}");
        }
        /// <summary>
        /// Send error log to COM debugger
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="sendMsgBox">Should debugger get message box</param>
        /// <param name="sender">Object that sends message</param>
        public static void Error(string message, bool sendMsgBox, object sender = null)
        {
            if (sender != null)
            {
                Cosmos.System.Global.Debugger.Send($"[Error] From {sender.GetType().Name}: {message}");
                if (sendMsgBox)
                    Cosmos.System.Global.Debugger.SendMessageBox($"Got error from {sender.GetType().ToString()}: {message}");
            }
            else
            {
                Cosmos.System.Global.Debugger.Send($"[Error] {message}");
                if (sendMsgBox)
                    Cosmos.System.Global.Debugger.SendMessageBox($"Got error: {message}");
            }
        }
        /// <summary>
        /// Send serve/critical log to COM debugger
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="executePanic">Should fall in kernel panic after sending log</param>
        /// <param name="sender">Object that sends message</param>
        public static void Critical(string message, bool executePanic = true, object sender = null)
        {
            if (sender != null)
            {
                Cosmos.System.Global.Debugger.Send($"[Serve] From {sender.GetType().Name}: {message}");
                Cosmos.System.Global.Debugger.SendMessageBox($"Got fatal error from {sender.GetType().ToString()}: {message}");
            }
            else
            {
                Cosmos.System.Global.Debugger.Send($"[Serve] {message}");
                Cosmos.System.Global.Debugger.SendMessageBox($"Got fatal error: {message}");
            }
            if (executePanic)
            {
                KernelPanic panic;
                if (sender != null)
                    panic = new(message, sender);
                else
                    panic = new(message, "Unknown source");
            }
        }
    }
}

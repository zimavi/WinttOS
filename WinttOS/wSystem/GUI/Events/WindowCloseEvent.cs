using WinttOS.wSystem.GUI.Components;
using WinttOS.wSystem.wAPI.Events;

namespace WinttOS.wSystem.GUI.Events
{
    internal class WindowCloseEvent : ICancelable
    {
        public bool IsCancelled { get; set; }

        public Window Window { get; }

        public WindowCloseEvent(Window window)
        {
            Window = window;
        }
    }
}

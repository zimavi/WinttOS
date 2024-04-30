using System;

namespace WinttOS.wSystem.Processing
{
    public class Task
    {
        public readonly Action Callback;

        public Task(Action callback)
        {
            Callback = callback;
        }
    }
}

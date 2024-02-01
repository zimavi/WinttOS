using System;

namespace WinttOS.wSystem.Benchmark
{
    public class Timer
    {
        private DateTime _start = DateTime.UtcNow;

        public void RestartTimer() =>
            _start = DateTime.UtcNow;


        public TimeSpan GetElapsedTime() => 
            DateTime.UtcNow - _start;
    }
}

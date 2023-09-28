using System;

namespace WinttOS.System.Benchmark
{
    public class Timer
    {
        private DateTime start = DateTime.UtcNow;

        public void RestartTimer() =>
            start = DateTime.UtcNow;


        public TimeSpan GetElapsedTime() => 
            DateTime.UtcNow - start;
    }
}

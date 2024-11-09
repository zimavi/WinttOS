using System;

namespace WinttOS.wSystem.Benchmark
{
    public class Stopwatch
    {
        private DateTime _startTime;
        private DateTime _stopTime;
        private bool _isRunning;

        public bool IsRunning => _isRunning;


        /// <summary>
        /// Starts stopwatch
        /// </summary>
        public void Start()
        {
            _startTime = DateTime.Now;
            _isRunning = true;
        }

        /// <summary>
        /// Stops stopwatch
        /// </summary>
        public void Stop()
        {
            _stopTime = DateTime.Now;
            _isRunning = false;
        }

        public TimeSpan TimeElapsed
        {
            get
            {
                if (_isRunning)
                {
                    return DateTime.Now - _startTime;
                }
                else
                {
                    return _stopTime - _startTime;
                }
            }
        }
    }
}

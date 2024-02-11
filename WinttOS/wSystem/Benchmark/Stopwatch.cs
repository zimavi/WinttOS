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
        /// <exception cref="InvalidOperationException"></exception>
        public void Start()
        {
            if(_isRunning)
            {
                throw new InvalidOperationException("Stopwatch is already running.");
            }
            else
            {
                _startTime = DateTime.Now;
                _isRunning = true;
            }
        }

        /// <summary>
        /// Stops stopwatch
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Stop()
        {
            if (!_isRunning)
            {
                throw new InvalidOperationException("Stopwatch is not running.");
            }
            else
            {
                _stopTime = DateTime.Now;
                _isRunning = false;
            }
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

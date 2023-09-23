using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.Benchmark
{
    [Obsolete("This class couse compile error", true)]
    public class BenchmarkTimer : IDisposable
    {
        private DateTime? start = DateTime.UtcNow;

        public void Dispose()
        {
            start = null;
        }

        public void RestartTimer() =>
            start = DateTime.UtcNow;


        public TimeSpan? GetElapsedTime() => 
            DateTime.UtcNow - start;
    }
}

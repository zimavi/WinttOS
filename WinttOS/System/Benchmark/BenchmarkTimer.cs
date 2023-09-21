using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.Benchmark
{
    public class BenchmarkTimer : IDisposable
    {
        private DateTime? start = DateTime.UtcNow;

        public void Dispose()
        {
            start = null;
        }

        public TimeSpan? GetElapsedTime()
            => DateTime.UtcNow - start;
    }
}

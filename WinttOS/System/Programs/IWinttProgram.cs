using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.Programs
{
    public interface IWinttProgram // interfaces aren't used for now
    {
        public string Run();

        public void Abort();
    }
}

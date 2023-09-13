using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Utils.Programs
{
    public interface IWinttCommandProgram // interfaces aren't used for now
    {
        public string Execute(string[] args);
    }
}

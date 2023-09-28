using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.System;

namespace WinttOS.Core.Utils.Kernel
{
    public class HardErrorResponseOption : SmartEnum<HardErrorResponseOption>
    {
        public static readonly HardErrorResponseOption OptionAbortRetryIgnore   = new("OptionAbortRetryIgnore", 0);
        public static readonly HardErrorResponseOption OptionOk                 = new("OptionOk", 1);
        public static readonly HardErrorResponseOption OptionOkCancel           = new("OptionOkCancel", 2);
        public static readonly HardErrorResponseOption OptionRetryCancel        = new("OptionRetryCancel", 3);
        public static readonly HardErrorResponseOption OptionYesNo              = new("OptionYesNo", 4);
        public static readonly HardErrorResponseOption OptionYesNoCancel        = new("OptionYesNoCancel", 5);
        public static readonly HardErrorResponseOption OptionShutdownSystem     = new("OptionShutdownSystem", 6);
        public static readonly HardErrorResponseOption OptionOkNoWait           = new("OptionOkNoWait", 7);
        public static readonly HardErrorResponseOption OptionCancelTryContinue  = new("OptionCancelTryContinue", 8);

        private HardErrorResponseOption(string name, int value) : base(name, value)
        { }
    }
}

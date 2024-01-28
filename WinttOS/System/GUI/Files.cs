using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.GUI
{
    public static class Files
    {
        [ManifestResourceStream(ResourceName = "WinttOS.Core.resources.cur.bmp")]
        public static readonly byte[] RawCursorImage;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base.Utils.Registry
{
    public class RegistryEntry<T>
    {
        public string Key { get; set; }

        public T Value { get; set; }
    }
}

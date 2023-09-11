using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Base.Utils.Registry
{
    public class WinttRegistry
    {
        public static WinttRegistry Registry { get; private set; }

        public List<RegistryKey<object>> RegistryKeys { get; set; }

        #region Constructors
        public WinttRegistry()
        {
        }
        public WinttRegistry(params RegistryKey<object>[] registryKeys)
        {
            foreach(var key in registryKeys)
            {
                RegistryKeys.Add(key);
            }
        }
        public WinttRegistry(List<RegistryKey<object>> registryKeys)
        {
            RegistryKeys = registryKeys;
        }

        #endregion
    }
}

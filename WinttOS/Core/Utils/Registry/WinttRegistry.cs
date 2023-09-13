using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Utils.Registry
{
    public class WinttRegistry
    {
        public static WinttRegistry Registry { get; internal set; }

        public List<RegistryFolder> Registries { get; private set; } 

        #region Constructors
        public WinttRegistry()
        {
            Registries = new()
            {
                new("HKEY_LOCAL_MACHINE"),
                new("HKEY_CURRENT_USER")
            };
        }

        #endregion

        public override string ToString()
        {
            string toReturn = "";
            foreach (RegistryFolder folder in Registries)
            {
                foreach(RegistryEntry<object> entry in folder.RegistryEntries)
                {
                    toReturn += $" : {folder.Name}/{entry.Key}\n";
                }
            }
            return toReturn;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Utils.Registry
{
    public class RegistryFolder
    {
        public string Name
        {
            get
            {
                return Name;
            }
            set
            {
                if (!forbidenToRename(Name))
                    Name = value;
            }
        }

        public List<RegistryEntry<object>> RegistryEntries { get; set; }

        public List<RegistryFolder> RegistryFolders { get; set; }

        public RegistryFolder(string name)
        {
            Name = name;
            RegistryEntries = new();
            RegistryFolders = new();
        }
        public RegistryFolder(string name, params RegistryEntry<object>[] entries)
        {
            Name = name;
            RegistryEntries = new();
            RegistryFolders = new();
            RegistryEntries.AddRange(entries);
        }
        public RegistryFolder(string name, List<RegistryEntry<object>> entries)
        {
            Name = name;
            RegistryEntries = entries;
            RegistryFolders = new();
        }
        private static bool forbidenToRename(in string name) =>
            name == "HKEY_LOCAL_MACHINE" || name == "HKEY_CURRENT_USER";
    }
}

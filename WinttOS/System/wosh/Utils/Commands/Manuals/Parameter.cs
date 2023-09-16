using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.wosh.Utils.Commands.Manuals
{
    public enum ParameterType : byte
    {
        Any,
        String,
        Integer,
        Boolean,
    }
    public struct Parameter
    {
        public string Name;
        public string Alias;
        public string Description;
        public ParameterType Accepts;
        public bool IsRequired = false;

        public Parameter(string Name, string Alias, string Description, ParameterType Accepts)
            : this(Name, Alias, Description, Accepts, false)
        {}
        public Parameter(string Name, string Alias, string Description, ParameterType Accepts, bool IsRequired)
        {
            this.Name = Name;
            this.Alias = Alias;
            this.Description = Description;
            this.Accepts = Accepts;
            this.IsRequired = IsRequired;
        }
    }
    public struct KeyWord
    {
        public string Name;
        public string Description;

        public KeyWord(string Name, string Description)
        {
            this.Name = Name;
            this.Description = Description;
        }
    }


}

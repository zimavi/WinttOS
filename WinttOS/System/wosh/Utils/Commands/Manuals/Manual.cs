using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.wosh.Utils.Commands.Manuals
{
    public class Manual
    {
        public List<Parameter> Parameters = new();
        public List<KeyWord> KeyWords = new();
        public string CommandDesc = "";

        private Manual()
        {
        }

        public class CommandManualBuilder
        {
            private Manual manual;

            public CommandManualBuilder() => manual = new Manual();

            public CommandManualBuilder Description(string CommandDescription)
            {
                manual.CommandDesc = CommandDescription;
                return this;
            }

            public CommandManualBuilder WithParameter(string Name, string Alias, string Description,
                ParameterType Accepts = ParameterType.Any, bool IsRequired = false)
            {
                manual.Parameters.Add(new Parameter(Name, Alias, Description, Accepts, IsRequired));
                return this;
            }

            public CommandManualBuilder WithKeyWord(string Name, string Description)
            {
                manual.KeyWords.Add(new KeyWord(Name, Description));
                return this;
            }

            public Manual Build()
            {
                return manual;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using WinttOS.Core.Utils.Sys;

namespace WinttOS.wSystem.Shell.Utils
{
    public class ArgParser
    {
        private Dictionary<string, Action<string>> _options;
        private HashSet<string> _flags;
        private List<string> _required;
        private Dictionary<string, string> _parsed;   
        private Dictionary<string, string> _aliases;

        public string HelpMessage { get; private set; }

        public ArgParser()
        {
            _options = new();
            _flags = new();
            _required = new();
            _parsed = new();
            _aliases = new();
        }

        public void AddOption(string option, Action<string> action, string description = "", params string[] aliases)
        {
            _options[option] = action;
            foreach(var alias in aliases)
            {
                _aliases[alias] = option;
            }
            if (!description.IsNullOrWhiteSpace())
            {
                HelpMessage += $"{option} <value>: {description}\n";
                foreach (var alias in aliases)
                {
                    HelpMessage += $"{alias} <value>: Alias for {option}\n";
                }
            }
        }

        public void AddFlag(string flag, string description = "", params string[] aliases)
        {
            _flags.Add(flag);
            foreach (var alias in aliases)
            {
                _aliases[alias] = flag;
            }
            if (!string.IsNullOrEmpty(description))
            {
                HelpMessage += $"{flag}: {description}\n";
                foreach (var alias in aliases)
                {
                    HelpMessage += $"{alias}: Alias for {flag}\n";
                }
            }
        }

        public void AddRequiredParam(string param, string description = "", params string[] aliases)
        {
            _required.Add(param);
            foreach (var alias in aliases)
            {
                _aliases[alias] = param;
            }
            if (!string.IsNullOrEmpty(description))
            {
                HelpMessage += $"{param} <value>: {description}\n";
                foreach (var alias in aliases)
                {
                    HelpMessage += $"{alias} <value>: Alias for {param}\n";
                }
            }
        }

        public void Parse(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (_aliases.ContainsKey(arg))
                {
                    arg = _aliases[arg];
                }

                if (_options.ContainsKey(arg) && i + 1 < args.Length)
                {
                    _options[arg](args[i + 1]);
                    _parsed[arg] = args[i + 1];
                    i++;
                }
                else if (_flags.Contains(arg))
                {
                    _parsed[arg] = "true";
                }
                else if (_required.Contains(arg) && i + 1 < args.Length)
                {
                    _parsed[arg] = args[i + 1];
                    i++;
                }
                else
                {
                    throw new ArgumentException($"Invalid argument: {args[i]}\n{HelpMessage}");
                }
            }

            foreach (var param in _required)
            {
                if (!_parsed.ContainsKey(param))
                {
                    throw new ArgumentException($"Missing required parameter: {param}\n{HelpMessage}");
                }
            }
        }

        public string GetValue(string optionOrParam)
        {
            if (_aliases.ContainsKey(optionOrParam))
            {
                optionOrParam = _aliases[optionOrParam];
            }
            return _parsed.ContainsKey(optionOrParam) ? _parsed[optionOrParam] : null;
        }

        public bool IsFlagSet(string flag)
        {
            if (_aliases.ContainsKey(flag))
            {
                flag = _aliases[flag];
            }
            return _parsed.ContainsKey(flag);
        }
    }
}

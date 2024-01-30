using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinttOS.System.wosh.bash
{
    public class BashInterpreter
    {
        private List<BashFunction> _functions;
        private List<BashCommand> _commands;

        private int _executionPtr; // used for current execution
        private int _executePtr; // used as pointer to last executed line (function call)
        private int _entryPoint;
        private List<string> _rawCode;
        private int _endOfFile;
        private string _path;


        public BashInterpreter()
        {
            _rawCode = new();
            _functions = new();
            _commands = new();
            _executePtr = 0;
            _executionPtr = 0;
            _entryPoint = 0;
            _endOfFile = 0;
        }

        public string Parse(string path)
        {
            if (!File.Exists(path))
                return "File does not exists!";

            foreach (var item in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;
                _rawCode.Add(item);
                _endOfFile = _rawCode.Count - 1;
            }


            int line = 0;

            if (_rawCode[line] != "#!/bin/bash")
                return "Files is not a bash script!";

            bool isSelectingFunc = false;
            bool hasEntryPoint = false;
            BashFunction func = new(); // for selecting 

            while (line + 1 < _rawCode.Count)
            {
                line++;

                _rawCode[line] = _rawCode[line].Trim();
                string[] tokens = _rawCode[line].Split(' ');

                if (tokens[0].StartsWith('#')) //comment
                {
                    continue;
                }
                if (isSelectingFunc)
                {
                    if (tokens[0] == "}")
                    {
                        func.LineEnd = line;
                        _functions.Add(func);
                        isSelectingFunc = false;
                    }
                    else
                    {
                        List<string> args = new();
                        for (int i = 1; i < tokens.Length; i++) { args.Add(tokens[i]); }
                        _commands.Add(new(tokens[0], line, args));
                    }
                }
                else
                {
                    if (tokens.Length >= 3 && tokens[1] == "()")
                    {
                        if (tokens[2] != "{")
                            return $"Invalid syntax on line {line + 1}";
                        func = new();
                        func.Name = tokens[0];
                        func.LineStart = line;

                        isSelectingFunc = true;
                    }
                    else
                    {
                        if (!hasEntryPoint)
                        {
                            hasEntryPoint = true;
                            _entryPoint = line;
                        }
                        List<string> args = new();
                        for (int i = 1; i < tokens.Length; i++) { args.Add(tokens[i]); }
                        _commands.Add(new(tokens[0], line, args));
                    }
                }
            }
            return "Done.";
        }

        public void Execute()
        {
            BashFunction current = null;
            bool isInFunc = false;

            _executionPtr = _entryPoint - 1;

            while (true)
            {
                _executionPtr++;

                if (isInFunc && current.LineEnd == _executionPtr)
                {
                    _executionPtr = _executePtr;
                    current = null;
                    isInFunc = false;
                    continue;
                }

                if (_executionPtr > _endOfFile)
                    break;

                foreach (var cmd in _commands)
                {
                    if (cmd.Line == _executionPtr)
                    {
                        foreach (var func in _functions)
                        {
                            if (func.Name == cmd.Name)
                            {
                                _executePtr = _executionPtr;
                                _executionPtr = func.LineStart;
                                current = func;
                                isInFunc = true;
                            }
                        }

                        if (cmd.Args.Count == 0)
                        {
                            Console.WriteLine("Execution -> '{0}' (no args)", cmd.Name);
                        }
                        else
                        {
                            Console.WriteLine("Execution -> '{0}' -> '{1}'", cmd.Name, string.Join(' ', cmd.Args.ToArray()));
                        }
                        break;
                    }
                }
            }
        }
    }
}

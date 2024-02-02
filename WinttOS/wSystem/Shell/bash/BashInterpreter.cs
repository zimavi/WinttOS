using System;
using System.Collections.Generic;
using System.IO;
using WinttOS.Core.Utils.Debugging;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Shell.Utils;

namespace WinttOS.wSystem.Shell.bash
{
    //[Obsolete(message:"Class causes kernel panic on parse", error:true)]
    public class BashInterpreter
    {
        private List<BashFunction> _functions;
        private List<BashCommand> _commands;

        private int _executionPtr; // used for current execution
        private Core.Utils.Sys.Stack<BashCallFrame> _callStack;
        private int _entryPoint;
        private List<string> _rawCode;
        private int _endOfFile;
        private string _path;


        public BashInterpreter()
        {
            _rawCode = new();
            _functions = new();
            _commands = new();
            _callStack = new();
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
                if (item.IsNullOrWhiteSpace())
                    continue;
                _rawCode.Add(item);
                _endOfFile = _rawCode.Count - 1;
            }


            int line = 0;

            if (_rawCode[line] != "#!/bin/bash/")
                return "Files is not a bash script!";

            bool isSelectingFunc = false;
            bool hasEntryPoint = false;
            BashFunction func = new(); // for selecting 

            while (line + 1 < _rawCode.Count)
            {
                line++;

                _rawCode[line] = _rawCode[line].Trim();
                //string[] tokens = _rawCode[line].Split(' ');
                string[] tokens = Misc.ParseCommandLine(_rawCode[line]).ToArray();

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
            WinttDebugger.Trace("Begin to execute BASH", this);

            BashFunction current = null;
            BashCallFrame frame;
            bool isInFunc = false;

            _executionPtr = _entryPoint - 1;

            while (true)
            {
                WinttDebugger.Trace("New call handle", this);

                _executionPtr++;

                if (isInFunc)
                {
                    if (_callStack.Count == 0)
                    {
                        WinttDebugger.Trace("Call stack is 0", this);

                        isInFunc = false;
                        continue;
                    }
                    else if (current.LineEnd == _executionPtr)
                    {
                        WinttDebugger.Trace("Reached end of function, poping call stack", this);

                        frame = _callStack.Pop();
                        _executionPtr = frame.CallLine;
                        continue;
                    }
                }

                if (_executionPtr > _endOfFile)
                    break;

                WinttDebugger.Trace("Going thorugh commands", this);

                foreach (var cmd in _commands)
                {
                    if (cmd.Line == _executionPtr)
                    {
                        WinttDebugger.Trace("Found one with same line, checking if it's a function", this);

                        foreach (var func in _functions)
                        {
                            if (func.Name == cmd.Name)
                            {
                                WinttDebugger.Trace("Executing function", this);

                                _callStack.Push(new(_executionPtr, func));
                                _executionPtr = func.LineStart;
                                current = func;
                                isInFunc = true;
                                continue;
                            }
                        }

                        WinttDebugger.Trace("Execution command", this);

                        if (cmd.Args.Count == 0)
                        {
                            WinttOS.CommandManager.ProcessInput(cmd.Name);
                        }
                        else
                        {
                            WinttOS.CommandManager.ProcessInput(cmd.Name + " " + string.Join(' ', cmd.Args.ToArray()));
                        }
                        break;
                    }
                }
            }
            WinttDebugger.Trace("Execution ended!", this);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WinttOS.Core.Utils.Sys;
using WinttOS.wSystem.Shell.Utils;

namespace WinttOS.wSystem.Shell.bash
{
    public sealed class BashInterpreter
    {
        private readonly List<BashFunction> _functions;
        private readonly List<BashCommand> _commands;
        private readonly List<BashVariable> _variables;

        private int _executionPtr; // used for current execution
        private readonly Core.Utils.Sys.Stack<BashCallFrame> _callStack;
        private int _entryPoint;
        private readonly List<string> _rawCode;
        private int _endOfFile;
        private string _path;


        public BashInterpreter()
        {
            _rawCode = new();
            _functions = new();
            _commands = new();
            _variables = new();
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
            }
            _endOfFile = _rawCode.Count - 1;

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
                        _commands.Add(new(tokens[0], line, args, isFunctionCommand: true));
                    }
                }
                else
                {
                    if (tokens.Length >= 3 && tokens[1] == "()")
                    {
                        if (tokens[2] != "{")
                            return $"Invalid syntax on line {line + 1}";
                        func = new()
                        {
                            Name = tokens[0],
                            LineStart = line
                        };

                        isSelectingFunc = true;
                    }
                    else if (tokens[0] == "if")
                    {
                        int ifEnd = FindMatchingEnd(line, _rawCode, "if", "fi");
                        List<string> args = new();
                        for (int i = 1; i < tokens.Length; i++) { args.Add(tokens[i]); }
                        _commands.Add(new(tokens[0], line, args, true, ifEnd));
                    }
                    else if (tokens[0] == "else")
                    {
                        var lastCommand = _commands.LastOrDefault();
                        if (lastCommand == null || !lastCommand.IsConditional)
                            return "No proceding if for else statement";

                        lastCommand.ElseLine = line;
                    }
                    else if (tokens[0] == "while")
                    {
                        int whileEnd = FindMatchingEnd(line, _rawCode, "while", "done");
                        List<string> args = new();
                        for (int i = 1; i < tokens.Length; i++) { args.Add(tokens[i]); }
                        _commands.Add(new(tokens[0], line, args, isLoop: true, loopEnd: whileEnd));
                        _commands.Add(new("done", whileEnd, args, isLoopEnd: true, loopEnd: line));
                    }
                    else if (tokens[0] == "fi" || tokens[0] == "done")
                        continue;
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

        private int FindMatchingEnd(int startLine, List<string> lines, string startToken, string endToken)
        {
            int depth = 0;
            for(int i = startLine; i < lines.Count; i++)
            {
                string trimmedLine = lines[i].Trim();
                if (trimmedLine.StartsWith(startToken))
                {
                    depth++;
                }
                else if (trimmedLine.StartsWith(endToken))
                {
                    depth--;
                    if(depth == 0)
                    {
                        return i;
                    }
                }
            }

            throw new Exception($"Matching {endToken} not found for {startToken} starting at line {startLine}");
        }

        public void Execute()
        {

            BashFunction current = null;
            BashCallFrame frame;
            bool isInFunc = false;

            _executionPtr = _entryPoint - 1;

            while (true)
            {

                _executionPtr++;

                if (isInFunc)
                {
                    if (_callStack.Count == 0)
                    {

                        isInFunc = false;
                        continue;
                    }
                    else if (current.LineEnd == _executionPtr)
                    {

                        frame = _callStack.Pop();
                        _executionPtr = frame.CallLine;
                        continue;
                    }
                }

                if (_executionPtr > _endOfFile)
                    break;


                foreach (var cmd in _commands)
                {
                    if (cmd.Line == _executionPtr)
                    {
                        if (!isInFunc && cmd.IsFunctionCommand)
                            break;
                        if (cmd.Name.EndsWith("++"))
                        {
                            string varName = cmd.Name.Trim('+');
                            int currentValue = int.Parse(GetVariable(varName));
                            SetVariable(varName, (currentValue + 1).ToString());
                            break;
                        }
                        else if (cmd.Name.EndsWith("--"))
                        {
                            string varName = cmd.Name.Trim('-');
                            int currentValue = int.Parse(GetVariable(varName));
                            SetVariable(varName, (currentValue - 1).ToString());
                            break;
                        }
                        else if (cmd.Name.Contains('='))
                        {
                            var parts = cmd.Name.Split('=');
                            if(parts.Length == 2)
                            {
                                var expressionResult = EvaluateExpression(parts[1].Trim());
                                SetVariable(parts[0], parts[1]);
                            }
                            break;
                        }
                        else if (cmd.IsConditional)
                        {
                            if (EvaluateCondition(cmd.Args))
                            {
                                break;
                            }
                            else
                            {
                                _executionPtr = cmd.ElseLine -1 ?? cmd.ConditionalEnd.Value - 1;
                                break;
                            }
                        }
                        else if (cmd.IsLoop)
                        {
                            if(EvaluateCondition(cmd.Args))
                            {
                                break;
                            }
                            else
                            {
                                _executionPtr = cmd.LoopEnd.Value;
                                break;
                            }
                        }
                        else if (cmd.IsLoopEnd)
                        {
                            _executionPtr = cmd.LoopEnd.Value - 1;
                            break;
                        }
                        bool found = false;
                        foreach (var func in _functions)
                        {
                            if (func.Name == cmd.Name)
                            {
                                _callStack.Push(new(_executionPtr, func));
                                _executionPtr = func.LineStart;
                                current = func;
                                isInFunc = true;
                                for(int i = 0; i < cmd.Args.Count; i++)
                                {
                                    SetVariable((i + 1).ToString(), cmd.Args[i].StartsWith('$') ? GetVariable(cmd.Args[i][1..]) : cmd.Args[i]);
                                }
                                SetVariable("#", cmd.Args.Count.ToString());
                                found = true;
                                break;
                            }
                        }

                        if (found)
                            break;

                        for(int i = 0; i < cmd.Args.Count; i++)
                        {
                            if (cmd.Args[i].StartsWith("$"))
                                cmd.Args[i] = GetVariable(cmd.Args[i][1..]);
                        }

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
        }

        private string EvaluateExpression(string v)
        {
            var parts = v.Split(' ');
            if(parts.Length == 3)
            {
                try
                {
                    int left = parts[0].StartsWith("$") ? int.Parse(GetVariable(parts[0][1..])) : int.Parse(parts[0]);
                    string op = parts[1];
                    int right = parts[2].StartsWith("$") ? int.Parse(GetVariable(parts[2][1..])) : int.Parse(parts[2]);

                    return op switch
                    {
                        "+" => (left + right).ToString(),
                        "-" => (left - right).ToString(),
                        "/" => (left / right).ToString(),
                        "*" => (left * right).ToString(),
                        "%" => (left % right).ToString(),
                        _ => v,
                    };
                }
                catch
                {
                    return v;
                }
            }

            return v;
        }

        private string GetVariable(string name)
        {
            foreach(var variable in _variables)
            {
                if(variable.Name == name)
                    return variable.Value;
            }

            return "";
        }

        private void SetVariable(string name, string value)
        {
            bool found = false;
            foreach (var variable in _variables)
            {
                if (variable.Name == name)
                {
                    if(value.StartsWith('$'))
                        value = GetVariable(value);
                    variable.Value = value;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                _variables.Add(new(name, value));
            }
        }

        private bool EvaluateCondition(List<string> args)
        {
            if (args.Count == 3)
            {
                var left = args[0].StartsWith("$") ? GetVariable(args[0][1..]) : args[0];
                var op = args[1];
                var right = args[2].StartsWith("$") ? GetVariable(args[2][1..]) : args[2];

                return op switch
                {
                    "==" => left == right,
                    "!=" => left != right,
                    ">" => int.Parse(left) > int.Parse(right),
                    "<" => int.Parse(left) < int.Parse(right),
                    ">=" => int.Parse(left) >= int.Parse(right),
                    "<=" => int.Parse(left) <= int.Parse(right),
                    _ => throw new Exception($"Unsupported operator: {op}"),
                };
            }
            throw new Exception("Invalid condition format");
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands
{
    public class ManCommand : Command
    {
        public ManCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager("man [command name] - shows list of manuals or command manual");
        }

        public override string Execute(string[] arguments)
        {
            //if(arguments.Length == 0)
            //{
            //    string returnStr = "All registrated manuals: \n";
            //    returnStr += String.Join('\n', ManCommandManager.getAllManuals().GetAllKeys().ToArray());
            //    return returnStr;
            //}
            //if (ManCommandManager.getAllManuals().Keys.FirstOrDefault(str => str.Contains(arguments[0])) != null)
            //{
            //    return ManCommandManager.getCommandManual(arguments[0]);
            //}

            if(arguments.Length == 0)
            {
                List<string> commandsWithManuals = new List<string>();
                foreach(Command command in WinttOS.CommandManager.GetCommandsListInstances())
                {
                    if (command.CommandManual.Any())
                        commandsWithManuals.Add(command.CommandName);
                    //if(!command.Manual2.IsNull())
                    //{
                    //    commandsWithManuals.Add(command.name);
                    //}
                }
                string returnStr = "List of commands with manuals:\n";
                returnStr += String.Join('\n', commandsWithManuals.ToArray());
                return returnStr;
            }

            foreach(Command command in WinttOS.CommandManager.GetCommandsListInstances())
            {
                if(command.CommandName == arguments[0])
                {
                    /*
                    if(!command.Manual2.IsNull())
                    {
                        string ret = null;
                        ret += command.Manual2.CommandDesc + "\n\n";

                        if(command.Manual2.KeyWords.Any())
                        {
                            ret += "Key words: \n";
                            foreach (KeyWord keyWord in command.Manual2.KeyWords)
                            {
                                ret += $"{keyWord.Name}\t{keyWord.Description}\n";
                            }

                        }
                        if (command.Manual2.Parameters.Any())
                        {
                            ret += "Params: \n";
                            foreach (Parameter parameter in command.Manual2.Parameters)
                            {
                                if (parameter.IsRequired)
                                {
                                    ret += $"{parameter.Name}|{parameter.Alias}\t{parameter.Description}\n";
                                }
                                else
                                    ret += $"{parameter.Name}|{parameter.Alias}\t(Optional) {parameter.Description}\n";
                            }
                        }
                        return ret;
                    }
                    */

                    if(command.CommandManual.Any())
                    {
                        return String.Join('\n', command.CommandManual.ToArray());
                    }
                    break;
                }
            }

            return $"There is no manual for {arguments[0]} command!";
        }
    }
}

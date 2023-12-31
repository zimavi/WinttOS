﻿using System.Collections.Generic;
using WinttOS.Core.Utils.System;
using WinttOS.System.Services;
using WinttOS.System.wosh.Utils.Commands;

namespace WinttOS.System.wosh.commands.Processing
{
    public class SystemCtlCommand : Command
    {
        public SystemCtlCommand(string name) : base(name, Users.User.AccessLevel.Administrator) 
        {
            HelpCommandManager.addCommandUsageStrToManager("systemctl [--help|-h] - get usage str (or use 'man systemctl')");
            CommandManual = new()
            {
                "  systemctl list - get list of services",
                "  systemctl status <service.name> - get service status"
            };
        }

        public override string Execute(string[] arguments)
        {
            if (arguments.Length == 0 || arguments[0] == "list")
            {
                List<string> result = new();
                foreach (var service in WinttOS.ServiceProvider.Services)
                    result.Add(service.ProcessName);
                return string.Join('\n', result.ToArray());
            }
            if (arguments[0] == "-h" || arguments[0] == "--help")
                return string.Join('\n', CommandManual.ToArray());
            if (arguments.Length == 2 || arguments[0] == "status")
            {
                return "NotImplemented!";
                string errorMsg = null;
                ServiceStatus status = ServiceStatus.no_data;
                (status, errorMsg) = WinttOS.ServiceProvider.GetServiceStatus(arguments[1]);
                if (status == ServiceStatus.no_data)
                    return "Service not found!";
                if (string.IsNullOrEmpty(errorMsg))
                {
                    string statusOperator = status == ServiceStatus.OK ? "[X]" : "[ ]";
                    string res =
                        arguments[1] +
                        $"  {statusOperator} Status: {status}";
                    return res;
                }   
                else
                {
                    string statusOperator = status == ServiceStatus.OK ? "[X]" : "[ ]";
                    string res =
                        arguments[1] +
                        $"  {statusOperator} Status: {status}" +
                        $"  {errorMsg}";
                    return res;
                }
            }
            return null;
        }
    }
}

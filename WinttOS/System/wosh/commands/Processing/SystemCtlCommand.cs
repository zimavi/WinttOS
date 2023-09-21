using System.Collections.Generic;
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
                "\tsystemctl status <service.name>",
            };
        }

        public override string Execute(string[] arguments)
        {
            if (arguments.Length == 0 || arguments[0] == "list")
            {
                string result = string.Empty;
                foreach (var service in WinttOS.ServiceProvider.Services)
                    result += service.ProcessName + '\n';
                return result.Substring(0, result.Length -1);
            }
            if (arguments.Length == 2 || arguments[0] == "status")
            {
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

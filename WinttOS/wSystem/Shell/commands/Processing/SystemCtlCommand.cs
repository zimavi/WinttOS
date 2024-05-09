using System;
using System.Collections.Generic;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Services;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Processing
{
    public sealed class SystemCtlCommand : Command
    {
        public SystemCtlCommand(string[] name) : base(name, User.AccessLevel.Administrator) 
        {
            CommandManual = new()
            {
                "Avaiable commands:",
                "- systemctl {--list|-l} - get list of services",
                "- systemctl {--status|-s} <service.name> - get service status"

            };
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "--list" || arguments[0] == "-l")
            {
                List<string> result = new();
                foreach (var service in WinttOS.ServiceManager.Services)
                    result.Add(service.ProcessName);
                SystemIO.STDOUT.PutLine(string.Join('\n', result.ToArray()));
            }
            if (arguments[0] == "-h" || arguments[0] == "--help")
                SystemIO.STDOUT.PutLine(string.Join('\n', CommandManual.ToArray()));
            if (arguments.Count == 2 && (arguments[0] == "--status" || arguments[0] == "-s"))
            {
                string errorMsg;
                ServiceStatus status;
                (status, errorMsg) = WinttOS.ServiceManager.GetServiceStatus(arguments[1]);
                if (status == ServiceStatus.no_data)
                    return new(this, ReturnCode.ERROR, "Service not found!");
                if (string.IsNullOrEmpty(errorMsg))
                {
                    string statusOperator = (status == ServiceStatus.OK) ? "[X] Online" : "[ ] Offline";
                    SystemIO.STDOUT.PutLine(arguments[1] + "  " + statusOperator + "\nStatus: " +
                        ServiceStatusFormatter.ToStringEnum(status));
                    return new(this, ReturnCode.OK);
                }   
                else
                {
                    string statusOperator = (status == ServiceStatus.OK) ? "[X] Online" : "[ ] Offline";
                    SystemIO.STDOUT.PutLine(arguments[1] + "  " + statusOperator + "\nStatus: " + 
                        ServiceStatusFormatter.ToStringEnum(status) + "\nError: " + errorMsg);
                    return new(this, ReturnCode.OK);
                }
                
            }
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Please use 'man systemctl'!");
        }
    }
}

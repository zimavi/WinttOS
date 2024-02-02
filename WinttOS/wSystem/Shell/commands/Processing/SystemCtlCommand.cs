using System;
using System.Collections.Generic;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Processing
{
    public class SystemCtlCommand : Command
    {
        public SystemCtlCommand(string[] name) : base(name, User.AccessLevel.Administrator) 
        {
            CommandManual = new()
            {
                "Avaiable commands:",
                "- systemctl --list - get list of _services",
                //"- systemctl status <service.name> - get service status"

            };
        }

        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "--list")
            {
                List<string> result = new();
                foreach (var service in WinttOS.ServiceManager.Services)
                    result.Add(service.ProcessName);
                Console.WriteLine(string.Join('\n', result.ToArray()));
            }
            if (arguments[0] == "-h" || arguments[0] == "--help")
                Console.WriteLine(string.Join('\n', CommandManual.ToArray()));
            if (arguments.Count == 2 || arguments[0] == "--status")
            {
                return new(this, ReturnCode.ERROR, "NotImplemented!");
                /*
                string errorMsg = null;
                ServiceStatus status = ServiceStatus.no_data;
                (status, errorMsg) = WinttOS.ServiceManager.GetServiceStatus(arguments[1]);
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
                */
            }
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            Console.WriteLine("Please use 'man systemctl'!");
        }
    }
}

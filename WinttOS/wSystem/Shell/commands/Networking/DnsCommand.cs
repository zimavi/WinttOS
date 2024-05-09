using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4.UDP.DNS;
using Cosmos.System.Network.IPv4;
using System;
using System.Collections.Generic;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.commands.Networking
{
    public sealed class DnsCommand : Command
    {
        public DnsCommand(string[] commandValues) : base(commandValues)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            DnsClient xClient = new();
            string domainname;

            if (arguments.Count < 1 || arguments.Count > 2)
            {
                return new ReturnInfo(this, ReturnCode.ERROR_ARG);
            }
            else if (arguments.Count == 1)
            {
                xClient.Connect(DNSConfig.DNSNameservers[0]);
                SystemIO.STDOUT.PutLine("DNS used : " + DNSConfig.DNSNameservers[0].ToString());
                xClient.SendAsk(arguments[0]);
                domainname = arguments[0];
            }
            else
            {
                xClient.Connect(Address.Parse(arguments[0]));
                xClient.SendAsk(arguments[1]);
                domainname = arguments[1];
            }

            Address address = xClient.Receive();

            xClient.Close();

            if (address == null)
            {
                return new ReturnInfo(this, ReturnCode.ERROR, "Unable to find " + arguments[0]);
            }
            else
            {
                SystemIO.STDOUT.PutLine(domainname + " is " + address.ToString());
            }

            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("- dns {domain_name}");
            SystemIO.STDOUT.PutLine("- dns {dns_server_ip} {domain_name}");
        }
    }
}

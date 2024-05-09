using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DNS;
using System;
using System.Collections.Generic;
using WinttOS.wSystem.IO;

namespace WinttOS.wSystem.Shell.commands.Networking
{
    public sealed class PingCommand : Command
    {
        public PingCommand(string[] commandValues) : base(commandValues)
        { }

        public override ReturnInfo Execute(List<string> arguments)
        {
            int PacketSent = 0;
            int PacketReceived = 0;
            int PacketLost = 0;
            int PercentLoss;

            Address source;
            Address destination = Address.Parse(arguments[0]);

            if (destination != null)
            {
                source = IPConfig.FindNetwork(destination);
            }
            else //Make a DNS request if it's not an IP
            {
                var xClient = new DnsClient();
                xClient.Connect(DNSConfig.DNSNameservers[0]);
                xClient.SendAsk(arguments[0]);
                destination = xClient.Receive();
                xClient.Close();

                if (destination == null)
                {
                    return new ReturnInfo(this, ReturnCode.ERROR, "Failed to get DNS response for " + arguments[0]);
                }

                source = IPConfig.FindNetwork(destination);
            }

            try
            {
                SystemIO.STDOUT.PutLine("Sending ping to " + destination.ToString());

                var xClient = new ICMPClient();
                xClient.Connect(destination);

                for (int i = 0; i < 4; i++)
                {
                    xClient.SendEcho();

                    PacketSent++;

                    var endpoint = new EndPoint(Address.Zero, 0);

                    int second = xClient.Receive(ref endpoint, 4000);

                    if (second == -1)
                    {
                        SystemIO.STDOUT.PutLine("Destination host unreachable.");
                        PacketLost++;
                    }
                    else
                    {
                        if (second < 1)
                        {
                            SystemIO.STDOUT.PutLine("Reply received from " + endpoint.Address.ToString() + " time < 1s");
                        }
                        else if (second >= 1)
                        {
                            SystemIO.STDOUT.PutLine("Reply received from " + endpoint.Address.ToString() + " time " + second + "s");
                        }

                        PacketReceived++;
                    }
                }

                xClient.Close();
            }
            catch
            {
                return new ReturnInfo(this, ReturnCode.ERROR, "Ping process error.");
            }

            PercentLoss = 25 * PacketLost;

            SystemIO.STDOUT.PutLine("");
            SystemIO.STDOUT.PutLine("Ping statistics for " + destination.ToString() + ":");
            SystemIO.STDOUT.PutLine("    Packets: Sent = " + PacketSent + ", Received = " + PacketReceived + ", Lost = " + PacketLost + " (" + PercentLoss + "% loss)");

            return new ReturnInfo(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Usage:");
            SystemIO.STDOUT.PutLine("- ping {ip}");
            SystemIO.STDOUT.PutLine("- ping {domain_name}");
        }
    }
}

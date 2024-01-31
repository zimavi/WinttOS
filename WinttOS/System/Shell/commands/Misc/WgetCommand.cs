using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;
using Cosmos.System.Network.IPv4.UDP.DNS;
using System;
using System.Text;
using WinttOS.System.Shell.Utils.Commands;
using WinttOS.System.Users;

namespace WinttOS.System.Shell.Commands.Misc
{
    public class WgetCommand : Command
    {
        public WgetCommand(string name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager("wget <url/IPv4>");
        }

        public override string Execute(string[] arguments)
        {
            try
            {
                var dnsClient = new DnsClient();
                var tcpClient = new TcpClient(80);

                //Uri uri = new Uri(arguments[0]); Missing plugs

                dnsClient.Connect(DNSConfig.DNSNameservers[0]);
                dnsClient.SendAsk(arguments[0]);
                Address address = dnsClient.Receive();
                dnsClient.Close();

                tcpClient.Connect(address, 80);

                string httpget = "GET / HTTP/1.1\r\n" +
                                 "User-Agent: Wget (CosmosOS)\r\n" +
                                 "Accept: */*\r\n" +
                                 "Accept-Encoding: identity\r\n" +
                                 "Host: " + arguments[0] + "\r\n" +
                                 "Connection: Keep-Alive\r\n\r\n";

                tcpClient.Send(Encoding.ASCII.GetBytes(httpget));

                var ep = new EndPoint(Address.Zero, 0);
                var data = tcpClient.Receive(ref ep);
                tcpClient.Close();

                string httpresponse = Encoding.ASCII.GetString(data);

                Console.WriteLine(httpresponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
            }
            return base.Execute(arguments);
        }
    }
}

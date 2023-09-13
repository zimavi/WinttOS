using Cosmos.HAL;
using Cosmos.System.Network;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Commands;

namespace WinttOS.Core.commands
{
    public class ipconfigCommand : Command
    {

        public ipconfigCommand(string name) : base(name, Users.User.AccessLevel.Guest)
        {
            HelpCommandManager.addCommandUsageStrToManager(@"ipconfig - gets list of network devices");
            manual = new()
            {
                "Usage:",
                "ipconfig                   - get network information",
                "ipconfig [--release | -rl] - make current IP avaliable",
                "ipconfig [--ask]           - ask DHCP for new IP",
                "ipconfig [--list]          - get shorten info of network devices",
                "ipconfig [--set] <device> <IPv4/CIDR> [gateway]",
                "                           - set current IP manually",
                "ipconfig [--nameserver|-ns] <--add|--remove|-rm> <IPv4>",
                "                           - add or remove nameserver from config",
            };
        }

        /*
        public override string execute(string[] arguments)
        {

            string getnetwork = "";
            try
            {
                //getnetwork = Cosmos.HAL.NetworkDevice.Devices.ToString();
                foreach(Cosmos.HAL.NetworkDevice device in Cosmos.HAL.NetworkDevice.Devices)
                {
                    ushort ethCount = 0;
                    ushort wlanCount = 0;
                    string cardType;
                    if (device.CardType == Cosmos.HAL.CardType.Ethernet)
                    {
                        cardType = "eth" + ethCount++;
                    }
                    else
                    {
                        cardType = "wlan" + wlanCount++;
                    }

                    if (device.Ready)
                        getnetwork += $"{cardType}:  {device.Name} state READY";
                    else
                        getnetwork += $"{cardType}:  {device.Name} state NOT READY";

                    getnetwork += '\n';
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            return getnetwork;
        }
        */

        public override string execute(string[] arguments)
        {
            string returnStr = "";
            if (arguments.Length == 0)
            {
                if (NetworkStack.ConfigEmpty())
                    return "No network configuration detected! Use 'ipconfig -h' or 'ipconfig --help'";
                NetworkConfiguration.NetworkConfigs.ForEach((config) =>
                {
                    switch (config.Device.CardType)
                    {
                        case Cosmos.HAL.CardType.Ethernet:
                            returnStr += $"Ethernet Card : {config.Device.NameID} - {config.Device.Name}";
                            break;
                        case Cosmos.HAL.CardType.Wireless:
                            returnStr += $"Wireless Card : {config.Device.NameID} - {config.Device.Name}";
                            break;
                    }
                    if (NetworkConfiguration.CurrentNetworkConfig.Device == config.Device)
                        returnStr += " (current)";
                    returnStr += "\n";

                    returnStr += $"  MAC Address           : {config.Device.MACAddress.ToString()}\n";
                    returnStr += $"  IP Address            : {config.IPConfig.IPAddress.ToString()}\n";
                    returnStr += $"  Subnet mask           : {config.IPConfig.SubnetMask.ToString()}\n";
                    returnStr += $"  Default Gateway       : {config.IPConfig.DefaultGateway.ToString()}\n";
                    returnStr += "  DNS Nameservers       : \n";
                    DNSConfig.DNSNameservers.ForEach((namesrv) =>
                    {
                        returnStr += $"                        {namesrv.ToString()}";
                    });

                });
                return returnStr;
            }
            else if (arguments[0] == "--release" || arguments[0] == "-rl")
            {
                using(var xClient = new DHCPClient())
                {
                    xClient.SendReleasePacket();
                    xClient.Close();
                }
                NetworkConfiguration.ClearConfigs();
                return "Done.";
            }
            else if (arguments[0] == "--ask")
            {
                using (var xClient = new DHCPClient())
                {
                    if(xClient.SendDiscoverPacket() != -1)
                    {
                         returnStr = $"Configuration applied! You local IPv4 Address is {NetworkConfiguration.CurrentAddress}.";
                    }
                    else
                    {
                        NetworkConfiguration.ClearConfigs();
                        returnStr = "DHCP Discover failed. Can't apply dynamic IPv4 address.";
                    }

                    xClient.Close();
                }
                return returnStr;
            }
            else if (arguments[0] == "--list")
            {
                NetworkDevice.Devices.ForEach((device) =>
                {
                    switch (device.CardType)
                    {
                        case CardType.Ethernet:
                            returnStr += $"Ethernet Card - {device.NameID} - {device.Name} ({device.MACAddress})\n";
                            break;
                        case CardType.Wireless:
                            returnStr += $"Wireless Card - {device.NameID} - {device.Name} ({device.MACAddress})\n";
                            break;
                    }
                });
            }
            else if (arguments[0] == "--set")
            {
                if (arguments.Length == 3 || arguments.Length == 4)
                {
                    string[] adrNetwork = arguments[2].Split('/');
                    Address ip = Address.Parse(adrNetwork[0]);
                    NetworkDevice nic = NetworkDevice.GetDeviceByName(arguments[1]);
                    Address gw = null;
                    if (arguments.Length == 4)
                    {
                        gw = Address.Parse(arguments[3]);
                    }
                    int cidr;
                    try
                    {
                        cidr = int.Parse(adrNetwork[1]);
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    Address subnet = Address.CIDRToAddress(cidr);
                    if (nic == null)
                        return $"Couldn't find network device {arguments[1]}";

                    if (ip != null && subnet != null && gw != null)
                    {
                        IPConfig.Enable(nic, ip, subnet, gw);

                        return "Config OK!";
                    }
                    else if (ip != null && subnet != null)
                    {
                        IPConfig.Enable(nic, ip, subnet, ip);
                        return "Config OK!";
                    }
                    else
                        return "Can't parse IP addresses (make sure they are well formated).";
                }
                else
                    return "Usage: ipconfig --set <device> <IPv4/CIDR> [gateway]";
            }
            else if (arguments[0] == "--nameserver" || arguments[0] == "-ns")
            {
                if (arguments[1] == "--add")
                {
                    DNSConfig.Add(Address.Parse(arguments[2]));
                    return $"{arguments[2]} has been added to nameservers list.";
                }
                else if (arguments[1] == "--remove" || arguments[1] == "-rm")
                {
                    DNSConfig.Remove(Address.Parse(arguments[2]));
                    return $"{arguments[2]} has been removed from nameservers list.";
                }
                else
                    return "Usage: ipconfig --nameserver|-ns <--add|--remove|-rm> <IPv4>";
            }
            return "";
        }
    }
}

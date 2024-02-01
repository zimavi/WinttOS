using Cosmos.HAL;
using Cosmos.System.Network;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System;
using WinttOS.wSystem.Shell.Utils.Commands;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Networking
{
    public class IpConfigCommand : Command
    {

        public IpConfigCommand(string name) : base(name, User.AccessLevel.Guest)
        {
            HelpCommandManager.AddCommandUsageStrToManager(@"ipconfig - gets list of network devices");
            CommandManual = new()
            {
                "Available commands:",
                "- ipconfig {--list-devices|-ldv}      List network devices",
                "- ipconfig {--ask}                    Find the DHCP server and ask a new IP address",
                "- ipconfig {--release|-rl}            Tell the DHCP server to make the IP address available",
                "- ipconfig {--set}                    Manually set an IP Address",
                "     Usage:",
                "     - ipconfig --set {device} {IPv4} {Subnet} {Gateway}",
                "- ipconfig {--nameserver|-ns}         Manually set an DNS server",
                "     Usage:",
                "     - ipconfig {--nameserver|-ns} {--add|-a|--remove|-rm} {IPv4}",
        };
        }

        public override string Execute(string[] arguments)
        {
            string returnData = "";
            if (arguments.Length == 0)
            {
                if (NetworkStack.ConfigEmpty())
                {
                    return "No network configuration detected! Use ipconfig /help";
                }
                foreach (NetworkConfig config in NetworkConfiguration.NetworkConfigs)
                {
                    switch (config.Device.CardType)
                    {
                        case CardType.Ethernet:
                            returnData += $"Ethernet Card : {config.Device.NameID} - {config.Device.Name}";
                            break;
                        case CardType.Wireless:
                            returnData += $"Wireless Card : {config.Device.NameID} - {config.Device.Name}";
                            break;

                    }
                    if (NetworkConfiguration.CurrentNetworkConfig.Device == config.Device)
                    {
                        returnData += $" (current)\n";
                    }
                    else
                    {
                        returnData += '\n';
                    }

                    returnData += "MAC Address          : " + config.Device.MACAddress.ToString();
                    returnData += "IP Address           : " + config.IPConfig.IPAddress.ToString();
                    returnData += "Subnet mask          : " + config.IPConfig.SubnetMask.ToString();
                    returnData += "Default Gateway      : " + config.IPConfig.DefaultGateway.ToString();
                    returnData += "DNS Nameservers      : ";

                    foreach (Address nameServer in DNSConfig.DNSNameservers)
                    {
                        returnData += "                       " + nameServer.ToString();
                    }
                }
            }
            else if (arguments[0] == "--release" || arguments[0] == "-rl")
            {
                DHCPClient xClient = new();
                xClient.SendReleasePacket();
                xClient.Close();

                NetworkConfiguration.ClearConfigs();

                API.Environment.SetEnvironmentVariable("HAS_NETWORK_CONNECTION", false);
            }
            else if (arguments[0] == "--ask")
            {
                DHCPClient xClient = new();
                if (xClient.SendDiscoverPacket() != -1)
                {
                    xClient.Close();
                    API.Environment.SetEnvironmentVariable("HAS_NETWORK_CONNECTION", true);
                    return "Configuration applied! Your local IPv4 Address is " + NetworkConfiguration.CurrentAddress + ".";
                }
                else
                {
                    NetworkConfiguration.ClearConfigs();

                    xClient.Close();
                    return "DHCP Discover failed. Can't apply dynamic IPv4 address.";
                }
            }
            else if (arguments[0] == "--list-devices" || arguments[0] == "-ldv")
            {
                foreach (var device in NetworkDevice.Devices)
                {
                    switch (device.CardType)
                    {
                        case CardType.Ethernet:
                            returnData += "Ethernet Card - " + device.NameID + " - " + device.Name + " (" + device.MACAddress + ")\n";
                            break;
                        case CardType.Wireless:
                            returnData += "Wireless Card - " + device.NameID + " - " + device.Name + " (" + device.MACAddress + ")\n";
                            break;
                    }
                }
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
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                    Address subnet = Address.CIDRToAddress(cidr);

                    if (nic == null)
                    {
                        return "Couldn't find network device: " + arguments[1];
                    }

                    if (ip != null && subnet != null && gw != null)
                    {
                        IPConfig.Enable(nic, ip, subnet, gw);
                        API.Environment.SetEnvironmentVariable("HAS_NETWORK_CONNECTION", true);
                        return "Config OK!";
                    }
                    else if (ip != null && subnet != null)
                    {
                        IPConfig.Enable(nic, ip, subnet, ip);
                        API.Environment.SetEnvironmentVariable("HAS_NETWORK_CONNECTION", true);
                        return "Config OK!";
                    }
                    else
                    {
                        return "Can't parse IP addresses (make sure they are well formated).";
                    }
                }
                else
                {
                    return "Usage : ipconfig --set {device} {IPv4/CIDR} {Gateway|null}";
                }
            }
            else if (arguments[0] == "--nameserver" || arguments[0] == "-ns")
            {
                if (arguments[1] == "--add" || arguments[1] == "-a")
                {
                    DNSConfig.Add(Address.Parse(arguments[2]));
                    return arguments[2] + " has been added to nameservers.";
                }
                else if (arguments[1] == "--remove" || arguments[1] == "-rm")
                {
                    DNSConfig.Remove(Address.Parse(arguments[2]));
                    return "arguments[2] + \" has been removed from nameservers list.";
                }
            }
            else
            {
                return "Wrong usage, please type: man ipconfig";
            }

            return returnData;
        }
    }
}

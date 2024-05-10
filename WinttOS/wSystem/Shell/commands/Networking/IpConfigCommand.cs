using Cosmos.HAL;
using Cosmos.System.Network;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using System;
using System.Collections.Generic;
using WinttOS.wSystem.IO;
using WinttOS.wSystem.Users;

namespace WinttOS.wSystem.Shell.Commands.Networking
{
    public sealed class IpConfigCommand : Command
    {

        public IpConfigCommand(string[] name) : base(name, User.AccessLevel.Guest)
        {
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

        public override ReturnInfo Execute()
        {
            string returnData = "";
            if (NetworkStack.ConfigEmpty())
            {
                SystemIO.STDOUT.PutLine("No network configuration detected! Use ipconfig /help");
                return new(this, ReturnCode.OK);
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

                returnData += "MAC Address          : " + config.Device.MACAddress.ToString() + '\n';
                returnData += "IP Address           : " + config.IPConfig.IPAddress.ToString() + '\n';
                returnData += "Subnet mask          : " + config.IPConfig.SubnetMask.ToString() + '\n';
                returnData += "Default Gateway      : " + config.IPConfig.DefaultGateway.ToString() + '\n';
                returnData += "DNS Nameservers      : " + '\n';

                foreach (Address nameServer in DNSConfig.DNSNameservers)
                {
                    returnData += "                       " + nameServer.ToString() + '\n';
                }
            }

            SystemIO.STDOUT.PutLine(returnData);
            return new(this, ReturnCode.OK);
        }


        public override ReturnInfo Execute(List<string> arguments)
        {
            if (arguments[0] == "--release" || arguments[0] == "-rl")
            {
                DHCPClient xClient = new();
                xClient.SendReleasePacket();
                xClient.Close();

                NetworkConfiguration.ClearConfigs();

                wAPI.Environment.SetEnvironmentVariable("HAS_NETWORK_CONNECTION", false);
            }
            else if (arguments[0] == "--ask")
            {
                DHCPClient xClient = new();
                if (xClient.SendDiscoverPacket() != -1)
                {
                    xClient.Close();
                    wAPI.Environment.SetEnvironmentVariable("HAS_NETWORK_CONNECTION", true);
                    SystemIO.STDOUT.PutLine("Configuration applied! Your local IPv4 Address is " + NetworkConfiguration.CurrentAddress + ".");
                    return new(this, ReturnCode.OK); 
                }
                else
                {
                    NetworkConfiguration.ClearConfigs();

                    xClient.Close();
                    return new(this, ReturnCode.ERROR, "DHCP Discover failed. Can't apply dynamic IPv4 address.");
                }
            }
            else if (arguments[0] == "--list-devices" || arguments[0] == "-ldv")
            {
                foreach (var device in NetworkDevice.Devices)
                {
                    switch (device.CardType)
                    {
                        case CardType.Ethernet:
                            SystemIO.STDOUT.PutLine("Ethernet Card - " + device.NameID + " - " + device.Name + " (" + device.MACAddress + ")");
                            break;
                        case CardType.Wireless:
                            SystemIO.STDOUT.PutLine("Wireless Card - " + device.NameID + " - " + device.Name + " (" + device.MACAddress + ")");
                            break;
                    }
                }
            }
            else if (arguments[0] == "--set")
            {
                if (arguments.Count == 3 || arguments.Count == 4)
                {
                    string[] adrNetwork = arguments[2].Split('/');
                    Address ip = Address.Parse(adrNetwork[0]);
                    NetworkDevice nic = NetworkDevice.GetDeviceByName(arguments[1]);
                    Address gw = null;
                    if (arguments.Count == 4)
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
                        return new(this, ReturnCode.ERROR, e.Message);
                    }
                    Address subnet = Address.CIDRToAddress(cidr);

                    if (nic == null)
                    {
                        return new(this, ReturnCode.ERROR, "Couldn't find network device: " + arguments[1]);
                    }

                    if (ip != null && subnet != null && gw != null)
                    {
                        IPConfig.Enable(nic, ip, subnet, gw);
                        wAPI.Environment.SetEnvironmentVariable("HAS_NETWORK_CONNECTION", true);
                        SystemIO.STDOUT.PutLine("Config OK!");
                        return new(this, ReturnCode.OK);
                    }
                    else if (ip != null && subnet != null)
                    {
                        IPConfig.Enable(nic, ip, subnet, ip);
                        wAPI.Environment.SetEnvironmentVariable("HAS_NETWORK_CONNECTION", true);
                        SystemIO.STDOUT.PutLine("Config OK!");
                        return new(this, ReturnCode.OK);
                    }
                    else
                    {
                        return new(this, ReturnCode.ERROR_ARG, "Can't parse IP addresses (make sure they are well formated).");
                    }
                }
                else
                {
                    SystemIO.STDOUT.PutLine("Usage : ipconfig --set {device} {IPv4/CIDR} {Gateway|null}");
                    return new(this, ReturnCode.OK);
                }
            }
            else if (arguments[0] == "--nameserver" || arguments[0] == "-ns")
            {
                if (arguments[1] == "--add" || arguments[1] == "-a")
                {
                    DNSConfig.Add(Address.Parse(arguments[2]));
                    SystemIO.STDOUT.PutLine(arguments[2] + " has been added to nameservers.");
                    return new(this, ReturnCode.OK);
                }
                else if (arguments[1] == "--remove" || arguments[1] == "-rm")
                {
                    DNSConfig.Remove(Address.Parse(arguments[2]));
                    SystemIO.STDOUT.PutLine(arguments[2] + " has been removed from nameservers list.");
                    return new(this, ReturnCode.OK);
                }
            }
            else
            {
                SystemIO.STDOUT.PutLine("Wrong usage, please type: man ipconfig");
                return new(this, ReturnCode.OK);
            }
            return new(this, ReturnCode.OK);
        }

        public override void PrintHelp()
        {
            SystemIO.STDOUT.PutLine("Please type 'man ipconfig'!");
        }
    }
}

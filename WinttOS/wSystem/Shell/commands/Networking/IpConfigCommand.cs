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

        public IpConfigCommand(string[] name) : base(name, AccessLevel.Default)
        {
            CommandManual = new()
            {
                "",
                "NAME",
                "       ipconfig - Network interface configuration utility",
                "",
                "SYNOPSIS",
                "       ipconfig [--list | -l]",
                "                [--ask | -a]",
                "                [--release | -r]",
                "                [--set | -s] DEVICE_ID IPV4 SUBNET GATEWAY",
                "                [--nameserver | -ns] [--add | -a] [--remove | -rm] IPV4",
                "",
                "DESCRIPTION",
                "       The ipconfig command is a network interface configuration utility that allows users to manage network settings on their system.",
                "",
                "OPTIONS",
                "       --list, -l",
                "              Lists all Ethernet adapters and nameservers configured on the system.",
                "",
                "       --ask, -a",
                "              Sends an ask packet to DHCP to request network configuration.",
                "",
                "       --release, -r",
                "              Sends a release packet to DHCP to release the current network configuration.",
                "",
                "       --set, -s DEVICE_ID IPV4 SUBNET GATEWAY",
                "              Manually sets the IP address, subnet mask, and default gateway for the specified network device.",
                "",
                "       --nameserver, -ns",
                "              Manages the nameserver list for DNS resolution.",
                "",
                "       --add, -a",
                "              Adds the specified IPv4 address to the nameserver list for DNS resolution.",
                "",
                "       --remove, -rm",
                "              Removes the specified IPv4 address from the nameserver list for DNS resolution.",
                "",
                "ARGUMENTS",
                "       DEVICE_ID",
                "              Specifies the ID of the network device to configure when using the --set option.",
                "",
                "       IPV4     Specifies the IPv4 address to set or add when using the --set or --add options.",
                "",
                "       SUBNET   Specifies the subnet mask when using the --set option.",
                "",
                "       GATEWAY  Specifies the default gateway when using the --set option.",
                "",
                "EXAMPLES",
                "       To list all Ethernet adapters and nameservers:",
                "              $ ipconfig --list",
                "",
                "NAME",
                "       ipconfig - Network interface configuration utility",
                "",
                "SYNOPSIS",
                "       ipconfig [--list | -l]",
                "                [--ask | -a]",
                "                [--release | -r]",
                "                [--set | -s] DEVICE_ID IPV4 SUBNET GATEWAY",
                "                [--nameserver | -ns] [--add | -a] [--remove | -rm] IPV4",
                "",
                "DESCRIPTION",
                "       The ipconfig command is a network interface configuration utility that allows users to manage network settings on their system.",
                "",
                "OPTIONS",
                "       --list, -l",
                "              Lists all Ethernet adapters and nameservers configured on the system.",
                "",
                "       --ask, -a",
                "              Sends an ask packet to DHCP to request network configuration.",
                "",
                "       --release, -r",
                "              Sends a release packet to DHCP to release the current network configuration.",
                "",
                "       --set, -s DEVICE_ID IPV4 SUBNET GATEWAY",
                "              Manually sets the IP address, subnet mask, and default gateway for the specified network device.",
                "",
                "       --nameserver, -ns",
                "              Manages the nameserver list for DNS resolution.",
                "",
                "       --add, -a",
                "              Adds the specified IPv4 address to the nameserver list for DNS resolution.",
                "",
                "       --remove, -rm",
                "              Removes the specified IPv4 address from the nameserver list for DNS resolution.",
                "",
                "ARGUMENTS",
                "       DEVICE_ID",
                "              Specifies the ID of the network device to configure when using the --set option.",
                "",
                "       IPV4     Specifies the IPv4 address to set or add when using the --set or --add options.",
                "",
                "       SUBNET   Specifies the subnet mask when using the --set option.",
                "",
                "       GATEWAY  Specifies the default gateway when using the --set option.",
                "",
                "EXAMPLES",
                "       To list all Ethernet adapters and nameservers:",
                "              $ ipconfig --list",
                "",
                "       To send an ask packet to DHCP:",
                "              $ ipconfig --ask",
                "",
                "       To release the current network configuration:",
                "              $ ipconfig --release",
                "",
                "       To manually set the IP address, subnet mask, and default gateway:",
                "              $ ipconfig --set eth0 192.168.1.100 255.255.255.0 192.168.1.1",
                "",
                "       To add or remove an IPv4 address from the nameserver list:",
                "              $ ipconfig --nameserver --add 8.8.8.8",
                "              $ ipconfig --nameserver --remove 8.8.8.8",
                "",
                "AUTHOR",
                "       ZImaVI",
                ""
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
            if (arguments[0] == "--release" || arguments[0] == "-r")
            {
                DHCPClient xClient = new();
                xClient.SendReleasePacket();
                xClient.Close();

                NetworkConfiguration.ClearConfigs();

                wAPI.Environment.SetEnvironmentVariable("HAS_NETWORK_CONNECTION", false);
            }
            else if (arguments[0] == "--ask" || arguments[0] == "-a")
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
            else if (arguments[0] == "--list" || arguments[0] == "-l")
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
            else if (arguments[0] == "--set" || arguments[0] == "-s")
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

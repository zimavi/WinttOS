using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Base.Utils.Commands;

namespace WinttOS.Base.commands
{
    public class ipconfigCommand : Command
    {

        public ipconfigCommand(string name) : base(name, false)
        {
            HelpCommandManager.addCommandUsageStrToManager(@"ipconfig - gets list of network devices");
        }

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
    }
}

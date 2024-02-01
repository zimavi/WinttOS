using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DNS;
using CosmosHttp.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WinttOS.Core.Utils.Debugging;

namespace WinttOS.wSystem.Networking
{
    public static class Http
    {
        public static byte[] DownloadRawFile(string url)
        {
            if (url.StartsWith("https://"))
            {
                throw new WebException("HTTPS currently not supported, please use HTTP");
            }

            string path = extractPathFromUrl(url);
            string domainName = extractDomainNameFromUrl(url);

            DnsClient dnsClient = new();

            dnsClient.Connect(DNSConfig.DNSNameservers[0]);
            dnsClient.SendAsk(domainName);
            Address address = dnsClient.Receive();
            dnsClient.Close();

            HttpRequest req = new();
            req.IP = address.ToString();
            req.Domain = domainName;
            req.Path = path;
            req.Method = "GET";
            req.Send();

            return req.Response.GetStream();
        }
        public static string DownloadFile(string url)
        {
            try
            {
                if (url.StartsWith("https://"))
                {
                    throw new WebException("HTTPS currently not supported, please use HTTP");
                }

                string path = extractPathFromUrl(url);
                string domainName = extractDomainNameFromUrl(url);

                DnsClient dnsClient = new();

                dnsClient.Connect(DNSConfig.DNSNameservers[0]);
                dnsClient.SendAsk(domainName);
                Address address = dnsClient.Receive();
                dnsClient.Close();

                HttpRequest req = new();
                req.IP = address.ToString();
                req.Domain = domainName;
                req.Path = path;
                req.Method = "GET";
                req.Send();

                return req.Response.Content;
            }
            catch(Exception e)
            {
                WinttDebugger.Error("Http: " + e.Message, true);
                Kernel.WinttRaiseHardError(Core.Utils.Kernel.WinttStatus.SYSTEM_THREAD_EXCEPTION_NOT_HANDLED,
                    null, Core.Utils.Kernel.HardErrorResponseOption.OptionShutdownSystem);
                return null;
            }
        }

        private static string extractPathFromUrl(string url)
        {
            int start = 0;
            if (url.Contains("://"))
            {
                start = url.IndexOf("://") + 3;
            }

            int idxOfSlash = url.IndexOf("/", start);
            if(idxOfSlash != -1)
            {
                return url.Substring(idxOfSlash);
            }
            return "/";
        }

        private static string extractDomainNameFromUrl(string url)
        {
            int start = 0;
            if (url.Contains("://"))
            {
                start = url.IndexOf("://") + 3;
            }
            int end = url.IndexOf("/", start);
            if(end == -1) 
            {
                end = url.Length;
            }

            return url[start..end];
        }
    }
}

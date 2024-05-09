using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.UDP.DNS;
using CosmosHttp.Client;
using System;
using System.Net;
using WinttOS.Core.Utils.Debugging;
using WinttOS.wSystem.IO;

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

            try
            {

                WinttDebugger.Trace("Http -> Preprocessing url");

                string path = ExtractPathFromUrl(url);
                string domainName = ExtractDomainNameFromUrl(url);

                WinttDebugger.Trace("Http -> Sending DNS request");

                DnsClient dnsClient = new();

                dnsClient.Connect(DNSConfig.DNSNameservers[0]);
                dnsClient.SendAsk(domainName);
                Address address = dnsClient.Receive();
                dnsClient.Close();

                if(address == null)
                {
                    SystemIO.STDOUT.PutLine("Error: Invalid received address (Is it null?)");
                    return null;
                }

                WinttDebugger.Trace("Http -> Sending GET request");

                HttpRequest req = new()
                {
                    IP = address.ToString(),
                    Domain = domainName,
                    Path = path,
                    Method = "GET"
                };
                req.Send();

                WinttDebugger.Trace("Http -> Sent request");

                return req.Response.GetStream();
            }
            catch(Exception ex)
            {
                WinttDebugger.Critical("Http -> " + ex.Message, true);
                return null;
            }
        }
        public static string DownloadFile(string url)
        {
            try
            {
                if (url.StartsWith("https://"))
                {
                    throw new WebException("HTTPS currently not supported, please use HTTP");
                }

                string path = ExtractPathFromUrl(url);
                string domainName = ExtractDomainNameFromUrl(url);

                DnsClient dnsClient = new();

                dnsClient.Connect(DNSConfig.DNSNameservers[0]);
                dnsClient.SendAsk(domainName);
                Address address = dnsClient.Receive();
                dnsClient.Close();

                HttpRequest req = new()
                {
                    IP = address.ToString(),
                    Domain = domainName,
                    Path = path,
                    Method = "GET"
                };
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

        public static string ExtractPathFromUrl(string url)
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

        public static string ExtractDomainNameFromUrl(string url)
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

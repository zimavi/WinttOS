using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.wSystem.API
{
    public static class Environment
    {
        private static Dictionary<string, string> EnvironmentVariables = new()
        {
            { "WINTT_DEBUG", "false" },
            { "HAS_NETWORK_CONNECTION", "false" }
        };

        /// <summary>
        /// Get Environment Variable
        /// </summary>
        /// <param name="key">value, or <see langword="null"/> if not exists</param>
        /// <returns></returns>
        public static string GetEnvironmentVariable(string key)
        {
            if (EnvironmentVariables.ContainsKey(key))
                return EnvironmentVariables[key];

            return null;
        }

        public static bool TryGetEnvironmentVariable(string key, out string value)
        {
            if (EnvironmentVariables.ContainsKey(key))
            {
                value = EnvironmentVariables[key];
                return true;
            }

            value = null;
            return false;
        }

        public static void SetEnvironmentVariable(string key, object value)
        {
            if(EnvironmentVariables.ContainsKey(key))
            {
                EnvironmentVariables[key] = value.ToString();
            }
            else
            {
                EnvironmentVariables.Add(key, value.ToString());
            }
        }
    }
}

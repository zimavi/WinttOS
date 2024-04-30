using System.Collections.Generic;

namespace WinttOS.wSystem.wAPI
{
    public static class Environment
    {
        private static Dictionary<string, string> _envVars = new()
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
            if (_envVars.ContainsKey(key))
                return _envVars[key];

            return null;
        }

        public static bool TryGetEnvironmentVariable(string key, out string value)
        {
            if (_envVars.ContainsKey(key))
            {
                value = _envVars[key];
                return true;
            }

            value = null;
            return false;
        }

        public static void SetEnvironmentVariable(string key, object value)
        {
            if(_envVars.ContainsKey(key))
            {
                _envVars[key] = value.ToString();
            }
            else
            {
                _envVars.Add(key, value.ToString());
            }
        }
    }
}

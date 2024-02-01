using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.wSystem.Json
{
    public class JsonObject
    {
        public List<string> Keys { get; private set; } = new();
        public List<string> Values { get; private set; } = new();

        public void Add(string key, string value)
        {
            Keys.Add(key);
            Values.Add(value);
        }

        public int Count => Keys.Count;

        public (string, string) this[int idx]
        {
            get
            {
                return (Keys[idx], Values[idx]);
            }
        }
    }
}

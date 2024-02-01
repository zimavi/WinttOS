using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.wSystem.Json
{
    public class JsonArray
    {
        public List<JsonObject> Objects = new();

        public JsonObject this[int idx]
        {
            get
            {
                return Objects[idx];
            }
        }
    }
}

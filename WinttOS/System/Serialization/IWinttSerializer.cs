using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.System.Serialization
{
    public interface IWinttSerializer<TSerializable>
    {
        public string Serialize(TSerializable data);
        public TSerializable Deserialize(string data);
        public string SerializeList(List<TSerializable> data);
        public List<TSerializable> DeserializeList(string data);
    }
}

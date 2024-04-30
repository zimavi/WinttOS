using System.Collections.Generic;

namespace WinttOS.wSystem.Serialization
{
    public interface IWinttSerializer<TSerializable>
    {
        public string Serialize(TSerializable data);
        public TSerializable Deserialize(string data);
        public string SerializeList(List<TSerializable> data);
        public List<TSerializable> DeserializeList(string data);
    }
}

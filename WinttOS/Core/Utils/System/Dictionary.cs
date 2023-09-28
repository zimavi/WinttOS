using System.Collections.Generic;

namespace WinttOS.Core.Utils.System
{
    public class Dictionary<TKey, TValue>
    {
        private List<TKey> keys = new();
        private List<TValue> values = new();

        public List<TKey> Keys => keys;
        public List<TValue> Values => values;

        public TValue this[TKey key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                values[keys.IndexOf(key)] = value;
            }
        }

        public int Count => keys.Count;

        public bool ContainsKey(TKey k) => 
            keys.Contains(k);

        public TValue Get(TKey key) => 
            values[keys.IndexOf(key)];

        public void Add(TKey key, TValue value)
        {
            keys.Add(key);
            values.Add(value);
        }

        public void Remove(TKey key)
        {
            keys.RemoveAt(keys.IndexOf(key));
            values.RemoveAt(keys.IndexOf(key));
        }

        public void Clear()
        {
            keys = new List<TKey>();
            values = new List<TValue>();
        }
    }
}

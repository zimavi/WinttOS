using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Utils
{
    public class Dictionary<TKey, TValue>
    {
        public List<TKey> Keys = new List<TKey>();
        public List<TValue> Values = new List<TValue>();

        public TValue this[TKey key]
        {
            get
            {
                return Get(key);
            }
            set
            {
                Values[Keys.IndexOf(key)] = value;
            }
        }

        public int Count
        {
            get
            {
                return Keys.Count;
            }
        }

        public bool ContainsKey(TKey k) => Keys.Contains(k);

        public TValue Get(TKey key) => Values[Keys.IndexOf(key)];

        public void Add(TKey key, TValue value)
        {
            Keys.Add(key);
            Values.Add(value);
        }

        public void Remove(TKey key)
        {
            Keys.RemoveAt(Keys.IndexOf(key));
            Values.RemoveAt(Keys.IndexOf(key));
        }

        public void Clear()
        {
            Keys = new List<TKey>();
            Values = new List<TValue>();
        }

        public List<TKey> GetAllKeys() => Keys;
    }
}

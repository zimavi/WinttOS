using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WinttOS.Core.Utils.System
{
    public abstract class SmartEnum<TEnum>
        : SmartEnum<TEnum, int>
        where TEnum : SmartEnum<TEnum, int>
    {
        protected SmartEnum(string name, int value) : base(name, value)
        {
        }
    }
    public abstract class SmartEnum<TEnum, TValue>
        where TEnum : SmartEnum<TEnum, TValue>
    {
        public readonly string Name;
        public readonly TValue Value;

        static readonly Dictionary<string, TEnum> fromName = new();
        static readonly Dictionary<TValue, TEnum> fromValue = new();

        protected SmartEnum(string name, TValue value)
        {
            Name = name;
            Value = value;
            fromName.Add(name, (TEnum)this);
            fromValue.Add(value, (TEnum)this);
        }

        public static bool operator ==(SmartEnum<TEnum, TValue> left, SmartEnum<TEnum, TValue> right) =>
            left.Value.Equals(right.Value);

        public static bool operator !=(SmartEnum<TEnum, TValue> left, SmartEnum<TEnum, TValue> right) =>
            !(left == right);

        public override bool Equals(object obj)
        {
            if (obj is not TEnum)
                return false;
            return Equals((TEnum)obj);
        }

        public static TEnum FromValue(TValue value)
        {
            foreach (var item in fromValue.Keys)
            {
                if (item.Equals(value))
                    return fromValue[item];
            }

            return null;
        }

        public static TEnum FromName(string name)
        {
            foreach (var item in fromName.Keys)
            {
                if (item.Equals(name))
                    return fromName[item];
            }
            return null;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString() => Name;
    }
}

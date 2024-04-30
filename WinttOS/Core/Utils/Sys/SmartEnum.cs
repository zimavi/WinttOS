using System.Collections.Generic;

namespace WinttOS.Core.Utils.Sys
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

        static readonly Dictionary<string, TEnum> _fromName = new();
        static readonly Dictionary<TValue, TEnum> _fromValue = new();

        protected SmartEnum(string name, TValue value)
        {
            Name = name;
            Value = value;
            _fromName.Add(name, (TEnum)this);
            _fromValue.Add(value, (TEnum)this);
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

        public static TEnum? FromValue(TValue value)
        {
            foreach (var item in _fromValue.Keys)
            {
                if (item.Equals(value))
                    return _fromValue[item];
            }

            return null;
        }

        public static TEnum? FromName(string name)
        {
            foreach (var item in _fromName.Keys)
            {
                if (item.Equals(name))
                    return _fromName[item];
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

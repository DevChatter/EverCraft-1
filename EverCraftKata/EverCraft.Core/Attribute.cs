using System;

namespace EverCraft.Core
{
    public struct Attribute
    {
        public static implicit operator Attribute(int value) => new Attribute(value);

        public Attribute(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
        public int Modifier => (int)Math.Floor((Value - 10) / 2f);
    }
}
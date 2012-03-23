using System;

namespace NetMX
{
    public class Number : IFormattable
    {
        private readonly decimal _value;

        public Number(decimal value)
        {
            _value = value;
        }

        public decimal Value
        {
            get { return _value; }
        }

        public static implicit operator decimal (Number number)
        {
            return number.Value;
        }

        public static implicit operator Number(decimal value)
        {
            return new Number(value);
        }

        public static Number operator +(Number left, Number right)
        {
            if (left == null || right == null)
            {
                return null;
            }
            return left.Value + right.Value;
        }

        public static Number operator -(Number left, Number right)
        {
            if (left == null || right == null)
            {
                return null;
            }
            return left.Value - right.Value;
        }

        public static Number operator *(Number left, Number right)
        {
            if (left == null || right == null)
            {
                return null;
            }
            return left.Value * right.Value;
        }

        public static Number operator /(Number left, Number right)
        {
            if (left == null || right == null)
            {
                return null;
            }
            return left.Value / right.Value;
        }

        public static bool operator >(Number left, Number right)
        {
            if (left == null || right == null)
            {
                return false;
            }
            return left.Value > right.Value;
        }

        public static bool operator <(Number left, Number right)
        {
            if (left == null || right == null)
            {
                return false;
            }
            return left.Value < right.Value;
        }

        public static bool operator <=(Number left, Number right)
        {
            if (left == null || right == null)
            {
                return false;
            }
            return left.Value <= right.Value;
        }

        public static bool operator >=(Number left, Number right)
        {
            if (left == null || right == null)
            {
                return false;
            }
            return left.Value >= right.Value;
        }        

        public bool Equals(Number other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other._value == _value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Number)) return false;
            return Equals((Number) obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }
    }
}
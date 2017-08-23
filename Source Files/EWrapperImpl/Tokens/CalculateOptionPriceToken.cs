using System;
using System.Threading;

namespace EWrapperImpl
{
    public class CalculateOptionPriceToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal CalculateOptionPriceToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal CalculateOptionPriceToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is CalculateOptionPriceToken)
            {
                return ID.Equals(((CalculateOptionPriceToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(CalculateOptionPriceToken a, CalculateOptionPriceToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(CalculateOptionPriceToken a, CalculateOptionPriceToken b)
        {
            return !a.Equals(b);
        }
    }
}
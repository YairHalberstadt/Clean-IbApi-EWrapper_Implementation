using System;
using System.Threading;

namespace EWrapperImpl
{
    public class CalculateImpliedVolatilityToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal CalculateImpliedVolatilityToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal CalculateImpliedVolatilityToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is CalculateImpliedVolatilityToken)
            {
                return ID.Equals(((CalculateImpliedVolatilityToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(CalculateImpliedVolatilityToken a, CalculateImpliedVolatilityToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(CalculateImpliedVolatilityToken a, CalculateImpliedVolatilityToken b)
        {
            return !a.Equals(b);
        }
    }
}
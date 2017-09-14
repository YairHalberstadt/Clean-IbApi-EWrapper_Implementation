using System;
using System.Threading;

namespace EWrapperImpl
{
    public class SoftDollarTiersToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal SoftDollarTiersToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal SoftDollarTiersToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is SoftDollarTiersToken)
            {
                return ID.Equals(((SoftDollarTiersToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(SoftDollarTiersToken a, SoftDollarTiersToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(SoftDollarTiersToken a, SoftDollarTiersToken b)
        {
            return !a.Equals(b);
        }
    }
}
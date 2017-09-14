using System;
using System.Threading;

namespace EWrapperImpl
{
    public class FundamentalDataToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal FundamentalDataToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal FundamentalDataToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is FundamentalDataToken)
            {
                return ID.Equals(((FundamentalDataToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(FundamentalDataToken a, FundamentalDataToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(FundamentalDataToken a, FundamentalDataToken b)
        {
            return !a.Equals(b);
        }
    }
}
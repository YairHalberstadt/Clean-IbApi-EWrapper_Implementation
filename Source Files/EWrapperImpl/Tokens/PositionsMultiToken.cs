using System;
using System.Threading;

namespace EWrapperImpl
{
    public class PositionsMultiToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal PositionsMultiToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal PositionsMultiToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is PositionsMultiToken)
            {
                return ID.Equals(((PositionsMultiToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(PositionsMultiToken a, PositionsMultiToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PositionsMultiToken a, PositionsMultiToken b)
        {
            return !a.Equals(b);
        }
    }
}
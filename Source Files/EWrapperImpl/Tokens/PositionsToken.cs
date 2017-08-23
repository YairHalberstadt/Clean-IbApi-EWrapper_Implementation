using System;
using System.Threading;

namespace EWrapperImpl
{
    public class PositionsToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal PositionsToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal PositionsToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is PositionsToken)
            {
                return ID.Equals(((PositionsToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(PositionsToken a, PositionsToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PositionsToken a, PositionsToken b)
        {
            return !a.Equals(b);
        }
    }
}
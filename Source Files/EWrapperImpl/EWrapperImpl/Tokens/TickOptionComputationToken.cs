using System;
using System.Threading;

namespace EWrapperImpl
{
    public class TickOptionComputationToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal TickOptionComputationToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal TickOptionComputationToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is TickOptionComputationToken)
            {
                return ID.Equals(((TickOptionComputationToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(TickOptionComputationToken a, TickOptionComputationToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(TickOptionComputationToken a, TickOptionComputationToken b)
        {
            return !a.Equals(b);
        }
    }
}
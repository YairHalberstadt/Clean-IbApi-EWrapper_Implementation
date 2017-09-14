using System;
using System.Threading;

namespace EWrapperImpl
{
    public class RealTimeBarsToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal RealTimeBarsToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal RealTimeBarsToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is RealTimeBarsToken)
            {
                return ID.Equals(((RealTimeBarsToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(RealTimeBarsToken a, RealTimeBarsToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(RealTimeBarsToken a, RealTimeBarsToken b)
        {
            return !a.Equals(b);
        }
    }
}
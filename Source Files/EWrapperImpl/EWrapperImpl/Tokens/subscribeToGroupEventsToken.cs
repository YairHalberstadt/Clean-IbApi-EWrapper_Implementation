using System;
using System.Threading;

namespace EWrapperImpl
{
    public class subscribeToGroupEventsToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal subscribeToGroupEventsToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal subscribeToGroupEventsToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is subscribeToGroupEventsToken)
            {
                return ID.Equals(((subscribeToGroupEventsToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(subscribeToGroupEventsToken a, subscribeToGroupEventsToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(subscribeToGroupEventsToken a, subscribeToGroupEventsToken b)
        {
            return !a.Equals(b);
        }
    }
}
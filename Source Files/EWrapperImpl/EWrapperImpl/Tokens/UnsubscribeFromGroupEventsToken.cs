using System;
using System.Threading;

namespace EWrapperImpl
{
    public class UnsubscribeFromGroupEventsToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal UnsubscribeFromGroupEventsToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal UnsubscribeFromGroupEventsToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is UnsubscribeFromGroupEventsToken)
            {
                return ID.Equals(((UnsubscribeFromGroupEventsToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(UnsubscribeFromGroupEventsToken a, UnsubscribeFromGroupEventsToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(UnsubscribeFromGroupEventsToken a, UnsubscribeFromGroupEventsToken b)
        {
            return !a.Equals(b);
        }
    }
}
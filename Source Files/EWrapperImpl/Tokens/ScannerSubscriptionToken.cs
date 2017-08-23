using System;
using System.Threading;

namespace EWrapperImpl
{
    public class ScannerSubscriptionToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal ScannerSubscriptionToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal ScannerSubscriptionToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is ScannerSubscriptionToken)
            {
                return ID.Equals(((ScannerSubscriptionToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(ScannerSubscriptionToken a, ScannerSubscriptionToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ScannerSubscriptionToken a, ScannerSubscriptionToken b)
        {
            return !a.Equals(b);
        }
    }
}
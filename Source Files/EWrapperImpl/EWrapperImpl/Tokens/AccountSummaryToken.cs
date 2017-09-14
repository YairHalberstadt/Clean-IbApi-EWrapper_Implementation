using System;
using System.Threading;

namespace EWrapperImpl
{
    public class AccountSummaryToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal AccountSummaryToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal AccountSummaryToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is AccountSummaryToken)
            {
                return ID.Equals(((AccountSummaryToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(AccountSummaryToken a, AccountSummaryToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(AccountSummaryToken a, AccountSummaryToken b)
        {
            return !a.Equals(b);
        }
    }
}
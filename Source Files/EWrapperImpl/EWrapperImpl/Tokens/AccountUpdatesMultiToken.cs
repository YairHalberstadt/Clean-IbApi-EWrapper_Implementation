using System;
using System.Threading;

namespace EWrapperImpl
{
    public class AccountUpdatesMultiToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal AccountUpdatesMultiToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal AccountUpdatesMultiToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is AccountUpdatesMultiToken)
            {
                return ID.Equals(((AccountUpdatesMultiToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(AccountUpdatesMultiToken a, AccountUpdatesMultiToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(AccountUpdatesMultiToken a, AccountUpdatesMultiToken b)
        {
            return !a.Equals(b);
        }
    }
}
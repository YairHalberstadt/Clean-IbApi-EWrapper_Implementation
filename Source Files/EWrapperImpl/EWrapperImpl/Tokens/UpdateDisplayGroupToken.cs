using System;
using System.Threading;

namespace EWrapperImpl
{
    public class UpdateDisplayGroupToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal UpdateDisplayGroupToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal UpdateDisplayGroupToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is UpdateDisplayGroupToken)
            {
                return ID.Equals(((UpdateDisplayGroupToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(UpdateDisplayGroupToken a, UpdateDisplayGroupToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(UpdateDisplayGroupToken a, UpdateDisplayGroupToken b)
        {
            return !a.Equals(b);
        }
    }
}
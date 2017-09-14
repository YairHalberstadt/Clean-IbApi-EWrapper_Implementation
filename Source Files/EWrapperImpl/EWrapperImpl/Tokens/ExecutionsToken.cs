using System;
using System.Threading;

namespace EWrapperImpl
{
    public class ExecutionsToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal ExecutionsToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal ExecutionsToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is ExecutionsToken)
            {
                return ID.Equals(((ExecutionsToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(ExecutionsToken a, ExecutionsToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ExecutionsToken a, ExecutionsToken b)
        {
            return !a.Equals(b);
        }
    }
}
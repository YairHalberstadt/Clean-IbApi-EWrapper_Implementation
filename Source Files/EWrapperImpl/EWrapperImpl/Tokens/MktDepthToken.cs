using System;
using System.Threading;

namespace EWrapperImpl
{
    public class MktDepthToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal MktDepthToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal MktDepthToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is MktDepthToken)
            {
                return ID.Equals(((MktDepthToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(MktDepthToken a, MktDepthToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(MktDepthToken a, MktDepthToken b)
        {
            return !a.Equals(b);
        }
    }
}
using System;
using System.Threading;

namespace EWrapperImpl
{
    public class SecDefOptParamsToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal SecDefOptParamsToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal SecDefOptParamsToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is SecDefOptParamsToken)
            {
                return ID.Equals(((SecDefOptParamsToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(SecDefOptParamsToken a, SecDefOptParamsToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(SecDefOptParamsToken a, SecDefOptParamsToken b)
        {
            return !a.Equals(b);
        }
    }
}
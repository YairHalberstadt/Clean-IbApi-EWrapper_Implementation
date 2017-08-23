using System;
using System.Threading;

namespace EWrapperImpl
{
    public class ContractDetailsToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal ContractDetailsToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal ContractDetailsToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is ContractDetailsToken)
            {
                return ID.Equals(((ContractDetailsToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(ContractDetailsToken a, ContractDetailsToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ContractDetailsToken a, ContractDetailsToken b)
        {
            return !a.Equals(b);
        }
    }
}
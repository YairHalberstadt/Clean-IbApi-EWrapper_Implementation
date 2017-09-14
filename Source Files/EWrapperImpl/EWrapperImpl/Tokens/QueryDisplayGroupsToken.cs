using System;
using System.Threading;

namespace EWrapperImpl
{
    public class QueryDisplayGroupsToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal QueryDisplayGroupsToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal QueryDisplayGroupsToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is QueryDisplayGroupsToken)
            {
                return ID.Equals(((QueryDisplayGroupsToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(QueryDisplayGroupsToken a, QueryDisplayGroupsToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(QueryDisplayGroupsToken a, QueryDisplayGroupsToken b)
        {
            return !a.Equals(b);
        }
    }
}
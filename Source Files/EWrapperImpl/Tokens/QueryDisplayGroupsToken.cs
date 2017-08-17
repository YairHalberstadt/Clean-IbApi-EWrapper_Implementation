using System;

namespace EWrapperImpl
{
    public class QueryDisplayGroupsToken : Token
    {
        public int ID { get; }
        private static int NextID=1;

        public QueryDisplayGroupsToken()
        {
            ID = NextID++;
        }

        internal QueryDisplayGroupsToken(int id) //don't request data with a token created with this constructor. 
                                        //It should only be used for searching for a specific token in a dictionary.
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
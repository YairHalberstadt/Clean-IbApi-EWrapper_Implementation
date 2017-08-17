using System;

namespace EWrapperImpl
{
    public class AccountSummaryToken : Token
    {
        public int ID { get; }
        private static int NextID=1;

        public AccountSummaryToken()
        {
            ID = NextID++;
        }

        internal AccountSummaryToken(int id) //don't request data with a token created with this constructor. 
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
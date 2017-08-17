using System;

namespace EWrapperImpl
{
    public class ContractDetailsToken : Token
    {
        public int ID { get; }
        private static int NextID=1;

        public ContractDetailsToken()
        {
            ID = NextID++;
        }

        internal ContractDetailsToken(int id) //don't request data with a token created with this constructor. 
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
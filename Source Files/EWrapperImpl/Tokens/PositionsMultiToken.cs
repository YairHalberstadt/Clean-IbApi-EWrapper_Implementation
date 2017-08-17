using System;

namespace EWrapperImpl
{
    public class PositionsMultiToken : Token
    {
        public int ID { get; }
        private static int NextID=1;

        public PositionsMultiToken()
        {
            ID = NextID++;
        }

        internal PositionsMultiToken(int id) //don't request data with a token created with this constructor. 
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
            if (obj is PositionsMultiToken)
            {
                return ID.Equals(((PositionsMultiToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(PositionsMultiToken a, PositionsMultiToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(PositionsMultiToken a, PositionsMultiToken b)
        {
            return !a.Equals(b);
        }
    }
}
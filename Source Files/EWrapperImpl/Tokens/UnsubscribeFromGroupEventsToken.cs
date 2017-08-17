using System;

namespace EWrapperImpl
{
    public class UnsubscribeFromGroupEventsToken : Token
    {
        public int ID { get; }
        private static int NextID=1;

        public UnsubscribeFromGroupEventsToken()
        {
            ID = NextID++;
        }

        internal UnsubscribeFromGroupEventsToken(int id) //don't request data with a token created with this constructor. 
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
            if (obj is UnsubscribeFromGroupEventsToken)
            {
                return ID.Equals(((UnsubscribeFromGroupEventsToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(UnsubscribeFromGroupEventsToken a, UnsubscribeFromGroupEventsToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(UnsubscribeFromGroupEventsToken a, UnsubscribeFromGroupEventsToken b)
        {
            return !a.Equals(b);
        }
    }
}
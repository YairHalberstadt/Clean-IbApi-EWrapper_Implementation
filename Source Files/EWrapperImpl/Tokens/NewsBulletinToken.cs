using System;

namespace EWrapperImpl
{
    public class NewsBulletinToken : Token
    {
        public int ID { get; }
        private static int NextID=1;

        public NewsBulletinToken()
        {
            ID = NextID++;
        }

        internal NewsBulletinToken(int id) //don't request data with a token created with this constructor. 
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
            if (obj is NewsBulletinToken)
            {
                return ID.Equals(((NewsBulletinToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(NewsBulletinToken a, NewsBulletinToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(NewsBulletinToken a, NewsBulletinToken b)
        {
            return !a.Equals(b);
        }
    }
}
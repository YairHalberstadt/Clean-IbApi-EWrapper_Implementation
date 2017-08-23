using System;
using System.Threading;

namespace EWrapperImpl
{
    public class NewsBulletinToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal NewsBulletinToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal NewsBulletinToken(int id)
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
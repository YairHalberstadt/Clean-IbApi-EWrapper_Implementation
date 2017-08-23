using System;
using System.Threading;

namespace EWrapperImpl
{
    public class HistoricalDataToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal HistoricalDataToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal HistoricalDataToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is HistoricalDataToken)
            {
                return ID.Equals(((HistoricalDataToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(HistoricalDataToken a, HistoricalDataToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(HistoricalDataToken a, HistoricalDataToken b)
        {
            return !a.Equals(b);
        }
    }
}
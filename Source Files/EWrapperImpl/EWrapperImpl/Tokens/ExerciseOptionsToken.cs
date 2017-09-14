using System;
using System.Threading;

namespace EWrapperImpl
{
    public class ExerciseOptionsToken : Token
    {
        public int ID { get; }
        private static int NextID=0;

        internal ExerciseOptionsToken()
        {
            ID = Interlocked.Increment(ref NextID);
        }

        internal ExerciseOptionsToken(int id)
        {
            ID = id;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is ExerciseOptionsToken)
            {
                return ID.Equals(((ExerciseOptionsToken)obj).ID);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(ExerciseOptionsToken a, ExerciseOptionsToken b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ExerciseOptionsToken a, ExerciseOptionsToken b)
        {
            return !a.Equals(b);
        }
    }
}
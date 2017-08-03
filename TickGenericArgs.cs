using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public class TickGenericArgs : TickArgs
    {
        public double Value { get; }

        public TickGenericArgs(int tickerId, int field, double value):base(tickerId,field)
        {
            Value = value;
        }
    }
}

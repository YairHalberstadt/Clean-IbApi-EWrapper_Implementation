using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public class TickSizeArgs : TickArgs
    {
        public double Size { get; }

        public TickSizeArgs(int tickerId, int field, int size) : base(tickerId, field)
        {
            Size = size;
        }
    }
}

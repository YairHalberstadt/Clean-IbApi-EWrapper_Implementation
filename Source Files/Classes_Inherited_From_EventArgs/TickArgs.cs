using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public class TickArgs :TickerIdArgs
    {
        public int Field { get; }
        public TickArgs(int tickerId, int field) : base(tickerId)
        {
            Field = field;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public class TickerIdArgs:EventArgs 
    {
        public int TickerID { get; }
        public TickerIdArgs(int tickerId)
        {
            TickerID = tickerId;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public class TickPriceArgs : TickArgs
    {
        public double Price { get; }
        public int CanAutoExecute { get; }

        public TickPriceArgs (int tickerId, int field, double price, int canAutoExecute) : base(tickerId,field)
        {
            Price = price;
            CanAutoExecute = canAutoExecute;
        }
    }
}

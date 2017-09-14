using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class TickPriceArgs :EventArgs
    {
       public MktDataToken Token { get; }
       public int Field { get; }
       public double Price { get; }
        public DateTime Time { get; }
       public int CanAutoExecute { get; }
       public TickPriceArgs(int tickerId, int field, double price, int canAutoExecute, DateTime time)
        {
            Token = new MktDataToken(tickerId);
            Field = field;
            Price = price;
            Time = time;
            CanAutoExecute = canAutoExecute;
        }
    }
}
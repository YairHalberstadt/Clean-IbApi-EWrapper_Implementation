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
       public int CanAutoExecute { get; }
       public TickPriceArgs(int tickerId, int field, double price, int canAutoExecute)
        {
            Token = new MktDataToken(tickerId);
            Field = field;
            Price = price;
            CanAutoExecute = canAutoExecute;
        }
    }
}
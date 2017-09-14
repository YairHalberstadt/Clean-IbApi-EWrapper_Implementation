using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class NextValidIdArgs :EventArgs
    {
       public int OrderId { get; }
       public NextValidIdArgs(int orderId)
        {
            OrderId = orderId;
        }
    }
}
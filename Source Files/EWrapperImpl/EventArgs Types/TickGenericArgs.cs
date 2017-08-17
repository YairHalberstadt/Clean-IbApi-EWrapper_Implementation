using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class TickGenericArgs :EventArgs
    {
       public MktDataToken Token { get; }
       public int Field { get; }
       public double Value { get; }
       public TickGenericArgs(int tickerId, int field, double value)
        {
            Token = new MktDataToken(tickerId);
            Field = field;
            Value = value;
        }
    }
}
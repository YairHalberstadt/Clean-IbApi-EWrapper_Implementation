using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class TickSizeArgs :EventArgs
    {
       public MktDataToken Token { get; }
       public int Field { get; }
       public int Size { get; }
       public TickSizeArgs(int tickerId, int field, int size)
        {
            Token = new MktDataToken(tickerId);
            Field = field;
            Size = size;
        }
    }
}
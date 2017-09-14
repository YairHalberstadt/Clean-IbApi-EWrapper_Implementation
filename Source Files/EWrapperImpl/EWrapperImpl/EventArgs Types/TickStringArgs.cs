using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class TickStringArgs :EventArgs
    {
       public MktDataToken Token { get; }
       public int Field { get; }
       public string Value { get; }
       public TickStringArgs(int tickerId, int field, string value)
        {
            Token = new MktDataToken(tickerId);
            Field = field;
            Value = value;
        }
    }
}
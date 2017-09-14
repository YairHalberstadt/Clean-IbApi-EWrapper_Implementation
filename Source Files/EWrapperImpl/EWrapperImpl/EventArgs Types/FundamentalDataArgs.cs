using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class FundamentalDataArgs :EventArgs
    {
       public FundamentalDataToken Token { get; }
       public string Data { get; }
       public FundamentalDataArgs(int reqId, string data)
        {
            Token = new FundamentalDataToken(reqId);
            Data = data;
        }
    }
}
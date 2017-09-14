using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class ExecDetailsEndArgs :EventArgs
    {
       public ExecutionsToken Token { get; }
       public ExecDetailsEndArgs(int reqId)
        {
            Token = new ExecutionsToken(reqId);
        }
    }
}
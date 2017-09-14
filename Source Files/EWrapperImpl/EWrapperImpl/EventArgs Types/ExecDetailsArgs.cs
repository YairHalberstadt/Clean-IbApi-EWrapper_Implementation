using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class ExecDetailsArgs :EventArgs
    {
       public ExecutionsToken Token { get; }
       public Contract Contract { get; }
       public Execution Execution { get; }
       public ExecDetailsArgs(int reqId, Contract contract, Execution execution)
        {
            Token = new ExecutionsToken(reqId);
            Contract = contract;
            Execution = execution;
        }
    }
}
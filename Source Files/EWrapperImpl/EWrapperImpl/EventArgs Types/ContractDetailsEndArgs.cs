using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class ContractDetailsEndArgs :EventArgs
    {
       public ContractDetailsToken Token { get; }
       public ContractDetailsEndArgs(int reqId)
        {
            Token = new ContractDetailsToken(reqId);
        }
    }
}
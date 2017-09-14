using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class ContractDetailsArgs :EventArgs
    {
       public ContractDetailsToken Token { get; }
       public ContractDetails ContractDetails { get; }
       public ContractDetailsArgs(int reqId, ContractDetails contractDetails)
        {
            Token = new ContractDetailsToken(reqId);
            ContractDetails = contractDetails;
        }
    }
}
using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class BondContractDetailsArgs :EventArgs
    {
       public ContractDetailsToken Token { get; }
       public ContractDetails Contract { get; }
       public BondContractDetailsArgs(int reqId, ContractDetails contract)
        {
            Token = new ContractDetailsToken(reqId);
            Contract = contract;
        }
    }
}
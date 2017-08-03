using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class BondContractDetailsArgs : EventArgs
{
public int ReqId { get;}
public ContractDetails Contract { get;}

public BondContractDetailsArgs(int reqId, ContractDetails contract)
{
ReqId = reqId;
Contract = contract;
}
}
}


using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class ContractDetailsArgs : EventArgs
{
public int ReqId { get;}
public ContractDetails ContractDetails { get;}

public ContractDetailsArgs(int reqId, ContractDetails contractDetails)
{
ReqId = reqId;
ContractDetails = contractDetails;
}
}
}


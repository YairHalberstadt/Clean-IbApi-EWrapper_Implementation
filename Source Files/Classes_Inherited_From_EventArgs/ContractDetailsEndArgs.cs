using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class ContractDetailsEndArgs : EventArgs
{
public int ReqId { get;}

public ContractDetailsEndArgs(int reqId)
{
ReqId = reqId;
}
}
}


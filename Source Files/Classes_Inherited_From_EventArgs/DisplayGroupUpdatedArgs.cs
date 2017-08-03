using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class DisplayGroupUpdatedArgs : EventArgs
{
public int ReqId { get;}
public string ContractInfo { get;}

public DisplayGroupUpdatedArgs(int reqId, string contractInfo)
{
ReqId = reqId;
ContractInfo = contractInfo;
}
}
}


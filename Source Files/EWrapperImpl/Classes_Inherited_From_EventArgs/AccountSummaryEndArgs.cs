using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class AccountSummaryEndArgs : EventArgs
{
public int ReqId { get;}

public AccountSummaryEndArgs(int reqId)
{
ReqId = reqId;
}
}
}


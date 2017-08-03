using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class ExecDetailsEndArgs : EventArgs
{
public int ReqId { get;}

public ExecDetailsEndArgs(int reqId)
{
ReqId = reqId;
}
}
}


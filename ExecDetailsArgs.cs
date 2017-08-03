using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class ExecDetailsArgs : EventArgs
{
public int ReqId { get;}
public Contract Contract { get;}
public Execution Execution { get;}

public ExecDetailsArgs(int reqId, Contract contract, Execution execution)
{
ReqId = reqId;
Contract = contract;
Execution = execution;
}
}
}


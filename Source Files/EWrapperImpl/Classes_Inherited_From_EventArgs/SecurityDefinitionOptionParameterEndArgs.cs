using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class SecurityDefinitionOptionParameterEndArgs : EventArgs
{
public int ReqId { get;}

public SecurityDefinitionOptionParameterEndArgs(int reqId)
{
ReqId = reqId;
}
}
}


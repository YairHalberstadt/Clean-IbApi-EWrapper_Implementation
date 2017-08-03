using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class AccountUpdateMultiEndArgs : EventArgs
{
public int RequestId { get;}

public AccountUpdateMultiEndArgs(int requestId)
{
RequestId = requestId;
}
}
}


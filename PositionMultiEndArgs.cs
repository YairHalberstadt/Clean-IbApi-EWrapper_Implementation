using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class PositionMultiEndArgs : EventArgs
{
public int RequestId { get;}

public PositionMultiEndArgs(int requestId)
{
RequestId = requestId;
}
}
}


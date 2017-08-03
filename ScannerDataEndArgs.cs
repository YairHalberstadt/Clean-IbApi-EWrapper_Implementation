using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class ScannerDataEndArgs : EventArgs
{
public int ReqId { get;}

public ScannerDataEndArgs(int reqId)
{
ReqId = reqId;
}
}
}


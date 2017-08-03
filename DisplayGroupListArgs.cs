using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class DisplayGroupListArgs : EventArgs
{
public int ReqId { get;}
public string Groups { get;}

public DisplayGroupListArgs(int reqId, string groups)
{
ReqId = reqId;
Groups = groups;
}
}
}


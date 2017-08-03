using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class TickSnapshotEndArgs : EventArgs
{
public int TickerId { get;}

public TickSnapshotEndArgs(int tickerId)
{
TickerId = tickerId;
}
}
}


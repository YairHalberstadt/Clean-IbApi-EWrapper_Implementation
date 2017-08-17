using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
    public class TickSnapshotEndArgs : EventArgs
    {
        public MktDataToken Token { get; }
        public TickSnapshotEndArgs(int tickerId)
        {
            Token = new MktDataToken(tickerId);
        }
    }
}
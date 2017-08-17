using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class PositionMultiEndArgs :EventArgs
    {
       public PositionsMultiToken Token { get; }
       public PositionMultiEndArgs(int requestId)
        {
            Token = new PositionsMultiToken(requestId);
        }
    }
}
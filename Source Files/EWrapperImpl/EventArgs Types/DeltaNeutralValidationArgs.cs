using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class DeltaNeutralValidationArgs :EventArgs
    {
       public int ReqId { get; }
       public UnderComp UnderComp { get; }
       public DeltaNeutralValidationArgs(int reqId, UnderComp underComp)
        {
            ReqId = reqId;
            UnderComp = underComp;
        }
    }
}
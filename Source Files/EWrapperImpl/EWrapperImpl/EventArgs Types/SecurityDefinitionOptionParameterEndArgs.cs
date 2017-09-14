using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class SecurityDefinitionOptionParameterEndArgs :EventArgs
    {
       public SecDefOptParamsToken Token { get; }
       public SecurityDefinitionOptionParameterEndArgs(int reqId)
        {
            Token = new SecDefOptParamsToken(reqId);
        }
    }
}
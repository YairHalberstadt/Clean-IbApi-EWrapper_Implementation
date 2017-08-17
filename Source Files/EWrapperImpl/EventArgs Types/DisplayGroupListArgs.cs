using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class DisplayGroupListArgs :EventArgs
    {
       public QueryDisplayGroupsToken Token { get; }
       public string Groups { get; }
       public DisplayGroupListArgs(int reqId, string groups)
        {
            Token = new QueryDisplayGroupsToken(reqId);
            Groups = groups;
        }
    }
}
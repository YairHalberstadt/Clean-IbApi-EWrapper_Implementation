using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class CurrentTimeArgs :EventArgs
    {
       public long Time { get; }
       public CurrentTimeArgs(long time)
        {
            Time = time;
        }
    }
}
using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class CommissionReportArgs :EventArgs
    {
       public CommissionReport CommissionReport { get; }
       public CommissionReportArgs(CommissionReport commissionReport)
        {
            CommissionReport = commissionReport;
        }
    }
}
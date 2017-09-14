using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class TickEfpArgs :EventArgs
    {
       public MktDataToken Token { get; }
       public int TickType { get; }
       public double BasisPoints { get; }
       public string FormattedBasisPoints { get; }
       public double ImpliedFuture { get; }
       public int HoldDays { get; }
       public string FutureLastTradeDate { get; }
       public double DividendImpact { get; }
       public double DividendsToLastTradeDate { get; }
       public TickEfpArgs(int tickerId, int tickType, double basisPoints, string formattedBasisPoints, double impliedFuture, int holdDays, string futureLastTradeDate, double dividendImpact, double dividendsToLastTradeDate)
        {
            Token = new MktDataToken(tickerId);
            TickType = tickType;
            BasisPoints = basisPoints;
            FormattedBasisPoints = formattedBasisPoints;
            ImpliedFuture = impliedFuture;
            HoldDays = holdDays;
            FutureLastTradeDate = futureLastTradeDate;
            DividendImpact = dividendImpact;
            DividendsToLastTradeDate = dividendsToLastTradeDate;
        }
    }
}
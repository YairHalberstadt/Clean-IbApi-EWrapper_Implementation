using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public class TickEfpArgs : TickerIdArgs
    {
        public double BasisPoints { get; }
        public string FormattedBasisPoints { get; }
        public double ImpliedFuture { get; }
        public int HoldDays { get; }
        public string FutureLastTradeDate { get; }
        public double DividendImpact { get; }
        public double DividendsToLastTradeDate { get; }

        public TickEfpArgs(int tickerId, int tickType, double basisPoints, string formattedBasisPoints, double impliedFuture, int holdDays, string futureLastTradeDate, double dividendImpact, double dividendsToLastTradeDate) : base(tickerId)
        {
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

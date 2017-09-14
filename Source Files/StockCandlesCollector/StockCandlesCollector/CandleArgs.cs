using EWrapperImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockCandlesCollector
{
    public class CandleArgs :EventArgs
    {
        public Candle Candle { get; }
        public CandleArgs(Candle candle)
        {
            Candle = candle;
        }
    }
}

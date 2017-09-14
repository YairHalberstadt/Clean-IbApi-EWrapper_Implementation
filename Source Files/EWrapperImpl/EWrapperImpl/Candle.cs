using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public class Candle
    {
        public DateTime StartTime;
        public CandleSize CandleSize;
        public double Open { get; }
        public double High { get; private set; }
        public double Low { get; private set; }
        public double Close { get; private set; }
        public bool Green { get => (Open < Close); }
        public bool Yellow { get => (Open == Close); }
        public bool Red { get => (Open > Close); }
        /// <summary>
        /// Creates a new instance of the Candle class.
        /// </summary>
        /// <param name="candleSize"></param>
        /// <param name="open"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="close"></param>
        /// <param name="startTime"></param>
        public Candle (CandleSize candleSize, double open, double high, double low, double close, DateTime startTime)
        {
            if(High<Open||High<Close||Low>Open||Low>Close)
            {
                throw (new ArgumentOutOfRangeException("high can not be lower than the open or the close, and low cannot be higher"));
            }
            CandleSize = candleSize;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            StartTime = startTime;
        }
        /// <summary>
        /// Creates an instance of the Candle class where no data is inputted except the start value. 
        /// Use this constructor when you create a candle to put live data into. You can then update the candle each
        /// time live data is recieved by calling the AddLatestValue method.
        /// </summary>
        /// <param name="candleSize"></param>
        /// <param name="startingValue"></param>
        /// <param name="startTime">leave empty to default to DateTime.Now</param>
        public Candle(CandleSize candleSize, double startingValue, DateTime startTime=default(DateTime))
        {
            CandleSize = candleSize;
            Open = High = Low = Close = startingValue;
            StartTime = startTime == default(DateTime) ? DateTime.Now : startTime;
        }
        /// <summary>
        /// Update the candle with the latest value. If it is greater/lower than the High/Low the High/Low will be readjsted.
        /// The close will then be givrn that value till the next value is recieved.
        /// </summary>
        /// <param name="value"></param>
        public void AddLatestValue(double value)
        {
            High = value > High ? value : High;
            Low = value < Low ? value : Low;
            Close = value;
        }

        public Candle Clone()
        {
            return new Candle(CandleSize, Open, High, Low, Close, StartTime);
        }
    }
}

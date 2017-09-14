using EWrapperImpl;
using IBApi;
using System;

namespace StockCandlesCollector
{
    public class CandlesCollector
    {
        public CandlesCollection Candles { get; }
        public Contract Contract { get; }
        public CandleSize CandleSize { get; }
        object LastPriceLock = new object();
        public double LastPrice { get { lock (LastPriceLock) { return _LastPrice; } } private set { lock (LastPriceLock) { _LastPrice = value; } } }
        double _LastPrice;
        IBWrapper IBWrapper;
        MktDataToken Token;

        Candle ActiveCandle;
        DateTime NextCandleTurnoverTime;
        public CandlesCollector(IBWrapper ibWrapper, CandleSize candleSize, Contract contract, Duration durationHistoricalCandles = Duration.Null, int? maxCandles = null)
        {
            CandleSize = candleSize;
            Candles = new CandlesCollection(candleSize, maxCandles);
            Contract = contract;
            IBWrapper = ibWrapper;

            StartHistoricalCollection(durationHistoricalCandles);
            if (CandleSize < CandleSize.Day)
            {
                NextCandleTurnoverTime = DateTime.Now.RoundUp(CandleSize.ToTimeSpan());
                Token = IBWrapper.Requester.ReqMarketData(contract, "", false, null, OnTickPrice);
            }
        }

        void StartHistoricalCollection(Duration duration)
        {
            if (duration != Duration.Null)
            {
                IBWrapper.Requester.ReqHistoricalData(Contract, DateTime.Now, duration, CandleSize, "MIDPOINT", false, false, null, OnHistoricalData);
            }
        }

        DateTime DateTimeOfLastPrice;
        object TickPriceLock = new object();

        public EventHandler<CandleArgs> NewCandleAdded = delegate{};
        void OnTickPrice(object sender, TickPriceArgs args)
        {
            bool fireEvent = false;
            CandleArgs candleArgs=null;
            lock (TickPriceLock)
            {
                if (args.Time >= NextCandleTurnoverTime)
                {
                    NextCandleTurnoverTime = args.Time.RoundUp(CandleSize.ToTimeSpan());
                    if (ActiveCandle != null)
                    {
                        Candles.Add(ActiveCandle);
                        fireEvent = true;
                        candleArgs = new CandleArgs(ActiveCandle);
                        ActiveCandle = new Candle(CandleSize, args.Price, NextCandleTurnoverTime - CandleSize.ToTimeSpan());
                    }
                }
                if (ActiveCandle == null)
                {
                    ActiveCandle = new Candle(CandleSize, args.Price, args.Time);
                }
                if (args.Time > DateTimeOfLastPrice)
                {
                    LastPrice = args.Price;
                    ActiveCandle.AddLatestValue(args.Price);
                    DateTimeOfLastPrice = args.Time;
                }
            }
            if (fireEvent) NewCandleAdded(this,candleArgs);
        }

        void OnHistoricalData(object sender, HistoricalDataArgs args)
        {
            Candles.Add(args.Candle);
        }
    }
}

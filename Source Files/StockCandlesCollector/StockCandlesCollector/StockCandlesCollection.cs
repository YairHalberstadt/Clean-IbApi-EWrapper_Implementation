using EWrapperImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace StockCandlesCollector
{
    /// <summary>
    /// Threadsafe collection for storing stockmarketcandles. 
    /// Candles can be added but not removed, but a fully read-write-able List containing clones of the candles can be returned from the 
    /// GetCopyOfCandles Method.
    /// 
    /// </summary>
    public class CandlesCollection : IReadOnlyList<Candle>
    {
        public int? MaxCandles { get; }
        public CandleSize CandleSize { get; }
        private List<Candle> Candles { get; set; }

        public int Count { get { lock (Lock) { return Candles.Count; } } }
        public Candle Last { get { lock (Lock) { return Candles.Last(); } } }
        object Lock = new object();
        public void Add(Candle candle)
        {
            
            if(candle.CandleSize!=CandleSize)
            {
                throw new ArgumentException("The CandleSize must match that of the rest of the collection");
            }
            lock (Lock)
            {
                if (MaxCandles != null && Candles.Count >= MaxCandles)
                {
                    Candles.RemoveAt(0);
                }

                int index = Candles.Select(c => c.StartTime).ToList().BinarySearch(candle.StartTime);
                if (index < 0) index = ~index;
                else throw new ArgumentOutOfRangeException("A candle is already in this Collection with the same start time");

                Candles.Insert(index, candle);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="candleSize">All Candles in the collection must be of this CandleSize</param>
        /// <param name="maxCandles">Once this number is reached any added candle will cause the earliest candle in the list to be removed. 
        /// Leave null to set no limit</param>
        public CandlesCollection(CandleSize candleSize,int? maxCandles=null)
        {
            Candles = new List<Candle>();
            CandleSize = candleSize;
            MaxCandles = maxCandles;
        }

        /// <summary>
        /// This returns a cached copy of the candles stored in the collector, in the form of a list.
        /// </summary>
        /// <returns>The cached copy.</returns>
        public List<Candle> GetCopyOfCandles()
        {
            lock (Lock)
            {
                return Candles.ConvertAll(c => c.Clone());
            }
        }

        public IEnumerator<Candle> GetEnumerator()
        {
            lock(Lock)
            {
                foreach(Candle candle in Candles)
                {
                    yield return candle;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Candle this[int key]
        {
            get
            {
                lock (Lock)
                {
                    return Candles[key];
                }
            }
            set
            {
                lock (Lock)
                {
                    Candles[key] = value;
                }
            }
        }

    }
}

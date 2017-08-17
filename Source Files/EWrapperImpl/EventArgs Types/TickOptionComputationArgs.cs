using IBApi;
using System;
using System.Collections.Generic; 

namespace EWrapperImpl
{
    public class TickOptionComputationArgs :EventArgs
    {
       public TickOptionComputationToken Token { get; }
       public int Field { get; }
       public double ImpliedVolatility { get; }
       public double Delta { get; }
       public double OptPrice { get; }
       public double PvDividend { get; }
       public double Gamma { get; }
       public double Vega { get; }
       public double Theta { get; }
       public double UndPrice { get; }
       public TickOptionComputationArgs(int tickerId, int field, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
        {
            Token = new TickOptionComputationToken(tickerId);
            Field = field;
            ImpliedVolatility = impliedVolatility;
            Delta = delta;
            OptPrice = optPrice;
            PvDividend = pvDividend;
            Gamma = gamma;
            Vega = vega;
            Theta = theta;
            UndPrice = undPrice;
        }
    }
}
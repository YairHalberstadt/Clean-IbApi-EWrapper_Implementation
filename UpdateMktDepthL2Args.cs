using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class UpdateMktDepthL2Args : EventArgs
{
public int TickerId { get;}
public int Position { get;}
public string MarketMaker { get;}
public int Operation { get;}
public int Side { get;}
public double Price { get;}
public int Size { get;}

public UpdateMktDepthL2Args(int tickerId, int position, string marketMaker, int operation, int side, double price, int size)
{
TickerId = tickerId;
Position = position;
MarketMaker = marketMaker;
Operation = operation;
Side = side;
Price = price;
Size = size;
}
}
}


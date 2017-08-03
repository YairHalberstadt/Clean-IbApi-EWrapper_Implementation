using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class UpdateMktDepthArgs : EventArgs
{
public int TickerId { get;}
public int Position { get;}
public int Operation { get;}
public int Side { get;}
public double Price { get;}
public int Size { get;}

public UpdateMktDepthArgs(int tickerId, int position, int operation, int side, double price, int size)
{
TickerId = tickerId;
Position = position;
Operation = operation;
Side = side;
Price = price;
Size = size;
}
}
}


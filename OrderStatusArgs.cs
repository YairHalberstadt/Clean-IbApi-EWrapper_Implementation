using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class OrderStatusArgs : EventArgs
{
public int OrderId { get;}
public string Status { get;}
public double Filled { get;}
public double Remaining { get;}
public double AvgFillPrice { get;}
public int PermId { get;}
public int ParentId { get;}
public double LastFillPrice { get;}
public int ClientId { get;}
public string WhyHeld { get;}

public OrderStatusArgs(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld)
{
OrderId = orderId;
Status = status;
Filled = filled;
Remaining = remaining;
AvgFillPrice = avgFillPrice;
PermId = permId;
ParentId = parentId;
LastFillPrice = lastFillPrice;
ClientId = clientId;
WhyHeld = whyHeld;
}
}
}


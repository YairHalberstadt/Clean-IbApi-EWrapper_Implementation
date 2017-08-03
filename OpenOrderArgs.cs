using IBApi;
using System;
using System.Collections.Generic;

namespace EWrapperImpl
{
public class OpenOrderArgs : EventArgs
{
public int OrderId { get;}
public Contract Contract { get;}
public Order Order { get;}
public OrderState OrderState { get;}

public OpenOrderArgs(int orderId, Contract contract, Order order, OrderState orderState)
{
OrderId = orderId;
Contract = contract;
Order = order;
OrderState = orderState;
}
}
}


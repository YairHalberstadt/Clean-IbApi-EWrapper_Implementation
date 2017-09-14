using System;

namespace EWrapperImpl
{
    /// <summary>
    /// For technical reasons (i.e. bad software design by IB) this token cant have the same ID as any TickOptionComputationToken.
    /// In essence this is almost functionally equivalent to the TickOptionComputationToken, the only reason I've created both
    /// is so it doesn't get confusing if a TickOptionComputationToken is returned from a marketData request.
    /// </summary>
    public class MktDataToken : TickOptionComputationToken
    {

        internal MktDataToken():base()
        {}

        internal MktDataToken(int id):base(id)
        { }
            


        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

    }
}
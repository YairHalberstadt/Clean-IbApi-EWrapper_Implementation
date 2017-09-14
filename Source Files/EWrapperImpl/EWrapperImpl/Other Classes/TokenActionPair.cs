using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    class TokenActionPair<token, Args> where token : Token
                                        where Args : EventArgs
    {
        public token Token { get; }
        public Action<object, Args> Action { get; }

        public TokenActionPair (token token, Action<object, Args> action)
        {
            Token = token;
            Action = action;
        }

        public override int GetHashCode()
        {
            return Token.GetHashCode()+Action.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is TokenActionPair<token, Args> taPair)
            {
                return Token.Equals(taPair.Token) && Action.Equals(taPair.Action);
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(TokenActionPair<token, Args> a, TokenActionPair<token, Args> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(TokenActionPair<token, Args> a, TokenActionPair<token, Args> b)
        {
            return !a.Equals(b);
        }

    }
}

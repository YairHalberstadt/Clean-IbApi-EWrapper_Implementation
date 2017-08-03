using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public class TickStringArgs: TickArgs
    {
        public string Value { get; }

        public TickStringArgs(int tickerId, int field, string value):base (tickerId,field)
        {
            Value=value;
        }
    }
}

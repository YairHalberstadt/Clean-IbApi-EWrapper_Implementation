using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWrapperImpl
{
    public class ID_StringErrorArgs : EventArgs
    {
        public int ID { get; }
        public int ErrorCode { get; }
        public string ErrorMsg { get; }

        public ID_StringErrorArgs(int id, int errorCode, string errorMsg)
        {
            ID = id;
            ErrorCode = errorCode;
            ErrorMsg = errorMsg;
        }
    }
}

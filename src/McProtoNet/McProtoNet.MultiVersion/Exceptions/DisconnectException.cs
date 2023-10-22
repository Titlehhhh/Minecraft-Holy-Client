using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McProtoNet.MultiVersion
{
    public class DisconnectException : Exception
    {
        public DisconnectException(string message) : base(message)
        {

        }
    }
}

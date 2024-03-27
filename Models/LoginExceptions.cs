using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficPolice
{
    public class LoginExceptions
    {
        public class IncorrectUsernameOrPasswordException : Exception { }
        public class CooldownException : Exception
        {
            public int RemainingTime { get; set; }
        }
    }
}

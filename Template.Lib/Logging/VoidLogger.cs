using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Logging
{
    public class VoidLogger : ILogger
    {
        public void Debug(string message)
        {
        }

        public void Error(string message)
        {
        }

        public void Fatal(string message)
        {
        }

        public void Info(string message)
        {
        }

        public void Trace(string message)
        {
        }

        public void Warn(string message)
        {
        }
    }
}

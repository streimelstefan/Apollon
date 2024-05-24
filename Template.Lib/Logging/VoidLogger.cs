using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Logging
{
    public class VoidLogger : ILogger
    {
        public int RecursionDepth 
        { 
            get
            {
                return 0;
            }

            set
            {
            }
        }
        public LogLevel Level { 
            get 
            {
                return LogLevel.Trace;
            } 

            set 
            { 
            } 
        }

        public ILogger CreateChild()
        {
            return new VoidLogger();
        }

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

        public void Silly(string message)
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

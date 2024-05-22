using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib.Logging
{
    public interface ILogger
    {
        public LogLevel Level { get; set; }

        public int RecursionDepth { get; set; }

        void Trace(string message);

        void Debug(string message);

        void Info(string message);

        void Warn(string message);

        void Error(string message);

        void Fatal(string message);
    }
}

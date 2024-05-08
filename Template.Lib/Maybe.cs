using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apollon.Lib
{
    public class Maybe<T, U>
    {
        public T? Value { get; private set; }

        public U? Error { get; private set; }

        public bool IsError
        {
            get
            {
                return Error != null;
            }
        }

        public bool IsSuccess
        {
            get
            {
                return Value != null;
            }
        }

        public Maybe(T value)
        {
            Value = value;
        }

        public Maybe(U error)
        {
            Error = error;
        }

        public Maybe(T? value, U? error)
        {
            if (value == null && error == null) throw new ArgumentException("Value and error are not allowed to be null at the same time.");
            if (value != null && error != null) throw new ArgumentException("Value and error are not allowed to be set at the same time.");
            Value = value;
            Error = error;
        }
    }
}

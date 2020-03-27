using System;
using System.Collections.Generic;
using System.Text;

namespace CSP
{
    class Variable<T>
    {
        public T Value {set; get;}

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

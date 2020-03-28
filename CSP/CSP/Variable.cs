using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.CSP
{
    class Variable<T>
    {

        public T Value { set; get; }
        public int Index { set; get; }

        public Variable() { }
        public Variable(Variable<T> src)
        {
            Value = src.Value;
            Index = src.Index;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

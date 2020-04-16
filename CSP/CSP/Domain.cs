using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSP.CSP
{
    class Domain<T>
    {
        public T[] Values { private set; get; }
        public T Default { set; get; } 

        public int Size { get { return Values.Length; } }

        public Domain(T[] domainArray)
        {
            Values = domainArray;
            
        }
        public Domain(Domain<T> domain)
        {
            Values = domain.Values;
            Default = domain.Default;
        }
        public Domain<T>DeepCopy()
        {
            var result = new Domain<T>(Values);
            return result;
        }

        public T UnsetValue()
        {
            return Values.Length == 1 ? Values[0] : Default;
        }
        public T[] AsArray()
        {
            return Values;
        }
    }
}

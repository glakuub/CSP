using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSP
{
    class Domain<T>
    {
        private T[] _domain;
        private int _currentIndex;
        public T Default { set; get; } 

        public int Size { get { return _domain.Length; } }

        public Domain(T[] domain)
        {
            _domain = domain;
        }
        public int RemainingSize()
        {
            return Size - _currentIndex;
        }
        public T First()
        {
            return _domain[0];
        }
        public bool HasNext()
        {

            return _currentIndex < _domain.Length;
        }
        public T Next()
        {
            T result = _domain[_currentIndex];
            _currentIndex++;
            return result;
        }
        public void Reset()
        {
            _currentIndex = 0;
        }
        public T UnsetValue()
        {
            return _domain.Length == 1 ? _domain[0] : Default;
        }
        public T[] AsArray()
        {
            return _domain;
        }
    }
}

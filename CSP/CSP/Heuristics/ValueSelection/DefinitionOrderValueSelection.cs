using System;
using System.Linq;

namespace CSP.CSP.Heuristics.ValueSelection
{
    class DefinitionOrderValueSelection<T> : IValueSelectionHeuristics<T>
    {
        private Domain<T> _domain;
        private bool[] _maskUsed;
        private int _currentIdx;
        public bool HasNext()
        {
            return _maskUsed.Any(e => e == false);
        }

        public T Next()
        {
            while(_currentIdx < _domain.Size && _maskUsed[_currentIdx]==true)
            {
                _currentIdx++;
            }
            if(_currentIdx < _domain.Size)
            {
                _maskUsed[_currentIdx] = true;
                return _domain.Values[_currentIdx++];
            }
            else
            {
                throw new Exception("There is no next.");
            }
        }

        //public void RegisterDomain(Domain<T> domain)
        //{
        //    _domain = domain;
        //}

        public void RegisterDomain(Variable<T> variable, Domain<T> domain, ref bool[] mask)
        {
            _domain = domain;
            _maskUsed = mask;
            for(int i = 0; i < _maskUsed.Length; i++)
            {
                if(_maskUsed[i] == false)
                {
                    _currentIdx = i;
                    break;
                }
            }
        }
    }
}

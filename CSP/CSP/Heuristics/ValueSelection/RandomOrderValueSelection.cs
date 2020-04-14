using CSP.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSP.CSP.Heuristics.ValueSelection
{
    class RandomOrderValueSelection<T> : IValueSelectionHeuristics<T>
    {
        private Domain<T> _domain;
        private bool[] _maskUsed;
        private Dictionary<int,int[]> _domainWasUsed;
        private int _currentIdx;
        private int[] _indexMap;

        public bool HasNext()
        {
            return _maskUsed.Any(e => e == false);
        }

        public T Next()
        {
            while (_currentIdx < _domain.Size && _maskUsed[_indexMap[_currentIdx]] == true)
            {
                _currentIdx++;
            }
            if (_currentIdx < _domain.Size)
            {
                _maskUsed[_indexMap[_currentIdx]] = true;
                return _domain.Values[_indexMap[_currentIdx]];
            }
            else
            {
                throw new Exception("There is no next.");
            }
        }

        public void RegisterDomain(Variable<T> variable, Domain<T> domain, ref bool[] mask)
        {
            if (_domainWasUsed == null)
                _domainWasUsed = new Dictionary<int, int[]>();
            
            if(_domainWasUsed.ContainsKey(variable.Index))
            {
                _indexMap = _domainWasUsed[variable.Index];
            }
            else 
            {
                _indexMap = Enumerable.Range(0, domain.Size).ToArray().Shuffle();
                _domainWasUsed.Add(variable.Index, _indexMap);
            }
            


            _domain = domain;
            _maskUsed = mask;
            for (int i = 0; i < _maskUsed.Length; i++)
            {
                if (_maskUsed[_indexMap[i]] == false)
                {
                    _currentIdx = i;
                    break;
                }
            }
        }
    }
}

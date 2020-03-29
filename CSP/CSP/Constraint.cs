using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSP.CSP
{
    class Constraint<T,S> where S:Variable<T>
    {
        private Func<T, T, bool> _predicate;
        private int _id1;
        private int _id2;
        
        public Constraint(int id1, int id2, Func<T, T, bool> predicate)
        {
          
            _id1 = id1;
            _id2 = id2;
            _predicate = predicate;
        }

        public bool Check(Tuple<S,Domain<T>,List<Constraint<T,S>>>[] varsWithCons, int[] indexMap)
        { 
            var v1 = varsWithCons[indexMap[_id1]].Item1.Value;
            var v2 = varsWithCons[indexMap[_id2]].Item1.Value;
            return _predicate(v1,v2);
        }
    }
}

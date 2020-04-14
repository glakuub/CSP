using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSP.CSP
{
    class Constraint<T,S> where S: Variable<T>
    {
        private Func<T, T, bool> _predicate;
        public int Id1 { private set; get; }
        public int Id2 { private set; get; }
        
        public Constraint(int id1, int id2, Func<T, T, bool> predicate)
        {
          
            Id1 = id1;
            Id2 = id2;
            _predicate = predicate;
        }

        public bool Check(Tuple<S,Domain<T>,List<Constraint<T,S>>>[] varsWithCons)
        { 
            var v1 = varsWithCons[Id1].Item1.Value;
            var v2 = varsWithCons[Id2].Item1.Value;
            return _predicate(v1,v2);
        }
        public bool Check(T v1, T v2)
        {
            return _predicate(v1, v2);
        }
    }
}

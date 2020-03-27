using System;
using System.Collections.Generic;
using System.Text;

namespace CSP
{
    class Constraint<T>
    {
        private Func<T, T, bool> _predicate;
        private int _arg1;
        private int _arg2;
        
        public Constraint(int arg1, int arg2, Func<T, T, bool> predicate)
        {
          
            _arg1 = arg1;
            _arg2 = arg2;
            _predicate = predicate;
        }
        //public bool Check(Variable<T>[,] variables)
        //{
        //    return _predicate.Invoke(variables[_arg1.Item1, _arg1.Item2].Value, variables[_arg2.Item1, _arg2.Item2].Value);
        //}
        public bool Check(Variable<T>[] variables)
        {
            var v1 = variables[_arg1];
            var v2 = variables[_arg2];
            return _predicate.Invoke(variables[_arg1].Value, variables[_arg2].Value);
        }
    }
}

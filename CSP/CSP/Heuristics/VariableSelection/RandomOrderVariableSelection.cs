using CSP.Util;
using CSP.CSP;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.CSP.Heuristics.VariableSelection
{
    class RandomOrderVariableSelection<T, S> :DefinitionOrderVariableSelection<T,S>, IVariableSelectionHeuristics<T, S> where S : Variable<T>
    {
        //private Stack<int> _selectedVariables;
        //private HashSet<int> _visited;
        //private Random _random;

        public RandomOrderVariableSelection():base()
        {

            //_visited = new HashSet<int>();
            //_selectedVariables = new Stack<int>();
            //_random = new Random();
        }
        //public bool HasNext()
        //{
        //    return _vars.Length - _selectedVariables.Count > 0;
        //}

        //public bool HasPrev()
        //{
        //    return _selectedVariables.Count > 0;
        //}

        //public S Next()
        //{
        //    int selected = _random.Next(0, _vars.Length);
        //    while(_selectedVariables.Contains(selected))
        //    {
        //        selected = _random.Next(0, _vars.Length);
        //    }
        //    _selectedVariables.Push(selected);
        //    _visited.Add(selected);
        //    return _vars[selected];
        //}

        //public S Prev()
        //{

        //    return _vars[_selectedVariables.Pop()];
        //}

        public new void RegisterVariables(S[] variables, Domain<T>[] domains = null)
        {
            base.RegisterVariables(variables);
            _vars = _vars.Shuffle();
        }

        //public S StartVariable()
        //{
        //    int selected = _random.Next(0, _vars.Length);
        //    _selectedVariables.Push(selected);
        //    _visited.Add(selected);
        //    return _vars[selected];
        //}

        public new bool IsBefore(S var1, S var2)
        {
            for(int i = 0; i < _vars.Length; i++)
            {
                if(_vars[i].Index == var2.Index)
                {
                    return false;
                }
                if(_vars[i].Index == var1.Index)
                {
                    return true;
                }
            }
            throw new Exception("Variables not found.");
        }
    }
}

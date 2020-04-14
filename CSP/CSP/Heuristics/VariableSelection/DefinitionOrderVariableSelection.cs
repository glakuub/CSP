namespace CSP.CSP.Heuristics.VariableSelection
{
    class DefinitionOrderVariableSelection<T,S> : VariableSelectionBase<T,S>, IVariableSelectionHeuristics<T, S> where S: Variable<T>
    {
       

       
        public bool HasNext()
        {
            return _currentIndex + 1 < _vars.Length;
        }

        public bool HasPrev()
        {
            return _currentIndex - 1 >= 0;
        }

        public bool IsBefore(S var1, S var2)
        {
            return var1.Index < var2.Index;
        }

        public S Next()
        {
            S result = _vars[_currentIndex + 1];
            _currentIndex++;
            return result;
        }

        public S Prev()
        {
            S result = _vars[_currentIndex - 1];
            _currentIndex--;
            return result;
        }

        public void RegisterVariables(S[] variables, Domain<T>[] domains=null)
        {
            _vars = new S[variables.Length];
            for (int i = 0; i < _vars.Length; i++)
            {
                _vars[i] = variables[i];
            }
        }

        public S StartVariable()
        { 
            return _vars[0];
        }
    }
}

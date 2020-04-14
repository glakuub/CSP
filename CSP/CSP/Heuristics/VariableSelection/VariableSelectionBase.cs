using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.CSP.Heuristics.VariableSelection
{
    class VariableSelectionBase<T,S> where S: Variable<T>
    {
        protected int _currentIndex;
        protected S[] _vars;
    }
}

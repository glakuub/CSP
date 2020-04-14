using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSP.CSP.Heuristics.VariableSelection
{
    class MostConstrainedVariableSelection<T,S>:DefinitionOrderVariableSelection<T,S>, IVariableSelectionHeuristics<T,S> where S:Variable<T>
    {
        public new void RegisterVariables(S[] variables, Domain<T>[] domains)
        {
            base.RegisterVariables(variables);
            int[] domainsSizes = domains.Select(e => e.Size).ToArray();
            _vars = _vars.OrderBy(e => domains[e.Index].Size).ToArray();
            
        }

        public new bool IsBefore(S var1, S var2)
        {
            for (int i = 0; i < _vars.Length; i++)
            {
                if (_vars[i].Index == var2.Index)
                {
                    return false;
                }
                if (_vars[i].Index == var1.Index)
                {
                    return true;
                }
            }
            throw new Exception("Variables not found.");
        }
    }
}

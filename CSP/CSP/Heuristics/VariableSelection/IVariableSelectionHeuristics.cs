namespace CSP.CSP.Heuristics.VariableSelection
{
    interface IVariableSelectionHeuristics<T,S> where S: Variable<T>
    {
        void RegisterVariables(S[] variables, Domain<T>[] domains);
        S StartVariable();
        bool HasNext();
        S Next();
        bool HasPrev();
        S Prev();

        bool IsBefore(S var1, S var2);
    }
}

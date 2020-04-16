namespace CSP.CSP.Heuristics.ValueSelection
{
    interface IValueSelectionHeuristics<T>
    {
        void RegisterDomain(Variable<T> variable, Domain<T> domain, ref bool[] mask);
        bool HasNext();
        T Next();
    }
}

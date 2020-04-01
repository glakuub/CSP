using CSP.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSP.CSP
{
    enum ValueSelection { DEFINITION, LEAST_CONSTRAINING}

 
    abstract class CSPBase<T,S> where S: Variable<T>
    {

        public Tuple<S, Domain<T>, List<Constraint<T,S>>>[] VariablesWithConstraints;
        protected int[] _indexMap;
        public string FileSaveDirectory { set; get; }
        public ValueSelection ValueSelection { set; get; }
        protected List<S[]> _foundSolutions;
                
        private Stopwatch _timeToFirst;
        private Stopwatch _timeToComplete;
        private int _iteration = 0;
        private int _visitedNodes = 0;
        private int _visitedNodesToFirst = 0;
        private int _foundSolutionsNumber = 0;
        private bool _hasSolution = false;

       
        public virtual void BacktrackingAlgorithm(bool printSolutions = false)
        {
            int alreadySet = 0;

            _timeToFirst = new Stopwatch();
            _timeToComplete = new Stopwatch();
            _foundSolutions = new List<S[]>();
            Variable<T> current = null;
            bool backtrack = false;

            _timeToFirst.Start();
            _timeToComplete.Start();
            while (true)
            {
                Domain<T> currentVariableDomain = null;
                if (current != null)
                {
                    
                    if (!backtrack)
                    {
                        if (HasNextVariable(current))
                        {
                            current = NextVariable(current);
                        }
                        else
                        {
                            _hasSolution = true;
                            //Console.WriteLine("found");
                            if (_foundSolutionsNumber == 0)
                            {
                                _visitedNodesToFirst = _visitedNodes;
                                _timeToFirst.Stop();
                            }
                            _foundSolutionsNumber++;
                            _foundSolutions.Add(SaveSolution());

                            
                            if(currentVariableDomain!=null && !HasNextDomainValue(current))
                                current = PreviousVariable(current);
                        }
                    }
                    else
                    {
                        if (HasPreviousVariable(current))
                        {
                            current = PreviousVariable(current);
                            backtrack = false;
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                }
                else
                {
                    current = StartVariable();
                    
                }

                
                currentVariableDomain = VariablesWithConstraints[_indexMap[current.Index]].Item2;

                bool constraintsSatisfied = false;
                if (!currentVariableDomain.HasNext())
                {
                    currentVariableDomain.Reset();
                    current.Value = currentVariableDomain.UnsetValue();
                    backtrack = true;
                   
                }
                else
                {
                    while (HasNextDomainValue(current))
                    {
                        var currentDomainValue = NextDomainValue(current);
                        current.Value = currentDomainValue;
                        _visitedNodes++;
                        if (CheckConstraints(current))
                        {
                            constraintsSatisfied = true;
                            break;
                        }
                    }
                    if (!constraintsSatisfied)
                    {
                        currentVariableDomain.Reset();
                        current.Value = currentVariableDomain.UnsetValue();
                        backtrack = true;
                    }
                }

                //if (_iteration % 1000000 == 0)
                //    alreadySet = AlredySetVariables();

                //if (alreadySet != AlredySetVariables())
                //{
                //    alreadySet = AlredySetVariables();
                //    Console.WriteLine(alreadySet);
                //}
                        _iteration++;
            }

            _timeToComplete.Stop();
            if (printSolutions)
            {
                PrintSolutionsInfo();
            }
            
            

        }

        public virtual void BacktrackingAlgorithmForwardCheck(bool printSolutions = false)
        {
            int alreadySet = 0;

            _timeToFirst = new Stopwatch();
            _timeToComplete = new Stopwatch();
            _foundSolutions = new List<S[]>();
            Variable<T> current = null;
            bool backtrack = false;

            _timeToFirst.Start();
            _timeToComplete.Start();


            var domainsStack = new Stack<Domain<T>[]>();
            Domain<T>[] domains = new Domain<T>[VariablesWithConstraints.Length];
            for (int i = 0; i < domains.Length; i++)
            {
                domains[i] = new Domain<T>(VariablesWithConstraints[i].Item2);
            }
            domainsStack.Push(domains);
            Domain<T>[] currentDomainsState = domainsStack.Peek();
            while (true)
            {
                Domain<T> currentVariableDomain = null;
                //currentDomainsState = domainsStack.Peek();
                if (current == null)
                    current = StartVariable();

                    if (backtrack)
                    {

                        if (HasPreviousVariable(current))
                        {
                            current = PreviousVariable(current);
                            backtrack = false;
                            domainsStack.Pop();
                            currentDomainsState = domainsStack.Peek();
                        }
                        else
                        {
                            break;
                        }
                       
                    }
                    else
                    {
                        if (HasNextVariable(current))
                        {
                            current = NextVariable(current);
                        }
                        else
                        {
                            _hasSolution = true;
                            Console.WriteLine("found");
                            if (_foundSolutionsNumber == 0)
                            {
                                _visitedNodesToFirst = _visitedNodes;
                                _timeToFirst.Stop();
                            }
                            _foundSolutionsNumber++;
                            _foundSolutions.Add(SaveSolution());
                        break; 
                        if (currentVariableDomain != null && !HasNextDomainValue(current))
                        {
                            current = PreviousVariable(current);
                            domainsStack.Pop();
                            currentDomainsState = domainsStack.Peek();
                        }
                        }
                    }



                currentVariableDomain = currentDomainsState[_indexMap[current.Index]];

                bool constraintsSatisfied = false;
                if (!currentVariableDomain.HasNext())
                {
                    currentVariableDomain.Reset();
                    current.Value = currentVariableDomain.UnsetValue();
                    backtrack = true;

                }
                else
                {

                    Domain<T>[] temp = new Domain<T>[currentDomainsState.Length];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        temp[i] = new Domain<T>(currentDomainsState[i]);
                    }
                    domainsStack.Push(temp);


                    while (currentVariableDomain.HasNext())
                    {
                        var currentDomainValue = currentVariableDomain.Next();
                        current.Value = currentDomainValue;
                        _visitedNodes++;

                        //Filter domains without values

                      
                        currentDomainsState = domainsStack.Peek();
                        currentVariableDomain = currentDomainsState[_indexMap[current.Index]];

                        if (!FilterOutDomains(current, ref currentDomainsState))
                        {

                            constraintsSatisfied = true;
                            break;
                           
                        }
                        
                    }
                    if (!constraintsSatisfied)
                    {
                        domainsStack.Pop();
                        //currentVariableDomain.Reset();
                        current.Value = currentVariableDomain.UnsetValue();
                        backtrack = true;
                       
                    }
                }

                //if (_iteration % 1000000 == 0)
                //    alreadySet = AlredySetVariables();

                //if (alreadySet != AlredySetVariables())
                //{
                //    alreadySet = AlredySetVariables();
                //    Console.WriteLine(alreadySet);
                //}
                _iteration++;
            }

            _timeToComplete.Stop();
            if (printSolutions)
            {
                PrintSolutionsInfo();
            }



        }
        private bool FilterOutDomains(Variable<T> current, ref Domain<T>[] currentDomainsState)
        {
            bool reducedToZero = false;
            
            var currentVariableConstraints = VariablesWithConstraints[_indexMap[current.Index]].Item3;
            for (int i = 0; i < currentVariableConstraints.Count; i++)
            {
                var pairVariable = VariablesWithConstraints[_indexMap[currentVariableConstraints[i].Id2]].Item1;
                
                var constraint = currentVariableConstraints[i];

                if (_indexMap[pairVariable.Index] > _indexMap[current.Index])
                {
                    var pairVariableDomain = currentDomainsState[_indexMap[pairVariable.Index]];


                    var domainArray = pairVariableDomain.AsArray();
                    int domainSize = domainArray.Length;
                    for (int j = 0; j < domainSize; j++)
                    {

                        if (!constraint.Check(current.Value, domainArray[j]))
                        {
                            pairVariableDomain.Used[j] = true;
                            domainSize--;
                        }

                    }
                    if (domainSize == 0)
                    {
                        //foreach(var v in checkedVariables)
                        //{
                        //    VariablesWithConstraints[_indexMap[v.Index]].Item2.Reset();
                        //}
                       
                        reducedToZero =  true;
                        break;
                    }

                   

                }

            }

            return reducedToZero;

        }
        private bool HasNextDomainValue(Variable<T> current)
        {
            var currentVariableDomain = VariablesWithConstraints[_indexMap[current.Index]].Item2;
            return !currentVariableDomain.IsEmpty();
        }
        private T NextDomainValue(Variable<T> current)
        {
            if(ValueSelection.Equals(ValueSelection.DEFINITION))
            {
                return DefinitionOrderValueSelection(current);
            }
            else
            {
                return LeastConstrainingValue(current);
            }
        }

        private T DefinitionOrderValueSelection(Variable<T> current)
        {
            var currentVariableDomain = VariablesWithConstraints[_indexMap[current.Index]].Item2;
            return currentVariableDomain.Next();

        }
        private T LeastConstrainingValue(Variable<T> current)
        {
            var currentVariableDomain = VariablesWithConstraints[_indexMap[current.Index]].Item2;
            int bestIndex = -1;
            T best = default(T);
            int leastConstraining = int.MaxValue;
            for (int i = 0; i < currentVariableDomain.Size; i++)
            {
                if (currentVariableDomain.Used[i] == false)
                {
                    var currentValue = currentVariableDomain.Values[i];
                    var currentConstraints = VariablesWithConstraints[_indexMap[current.Index]].Item3;
                    int excludesNumber = 0;
                    for (int j = 0; j < currentConstraints.Count; j++)
                    {
                        var coupledVarIdx = currentConstraints[j].Id2;
                        var coupledVarDomain = VariablesWithConstraints[_indexMap[coupledVarIdx]].Item2.Values;

                        for (int k = 0; k < coupledVarDomain.Length; k++)
                        {
                            if (!currentConstraints[j].Check(currentValue, coupledVarDomain[k]))
                                excludesNumber++;

                        }

                    }
                    if (excludesNumber < leastConstraining)
                    {
                        leastConstraining = excludesNumber;
                        best = currentValue;
                        bestIndex = i;
                    }
                }

            }
            currentVariableDomain.Used[bestIndex] = true;
            return best;
        }
        private string CreateInfoString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"\nvisited nodes to first solution: {_visitedNodesToFirst}");
            sb.AppendLine($"all visited nodes: {_visitedNodes}");
            sb.AppendLine($"found solution: {_hasSolution}");
            sb.AppendLine($"solutions number: {_foundSolutionsNumber}");
            sb.AppendLine($"time to first solution: {_timeToFirst.Elapsed}");
            sb.AppendLine($"time to complete: {_timeToComplete.Elapsed}\n");
            return sb.ToString();
        }
        protected void SaveSolutionsInfoToFile(string fileName)
        {
            var logger = new Logger(fileName);
            logger.WriteLine(CreateInfoString());
           
        }

        private void PrintSolutionsInfo()
        {
            Console.Write(CreateInfoString());
        }
        private S[] SaveSolution()
        {
            var saved = new S[VariablesWithConstraints.Length];
            for (int i = 0; i < saved.Length; i++)
            {
                saved[i] = (S)Activator.CreateInstance(typeof(S), new object[] { VariablesWithConstraints[i].Item1 });
            }

            return saved;
        }

        private Variable<T> StartVariable()
        {
            return VariablesWithConstraints[0].Item1;
        }
        private bool CheckConstraints(Variable<T> current)
        {
            bool result = true;
            int index = current.Index;
            var constraints = VariablesWithConstraints[_indexMap[current.Index]].Item3;
                foreach (var pred in constraints)
                {
                    if (!pred.Check(VariablesWithConstraints, _indexMap))
                    {
                        result = false;
                        break;
                    }
                }
            
            return result;
        }
        private bool HasPreviousVariable(Variable<T> current)
        { 
           // int currentIdx = Array.FindIndex(VariablesWithConstraints, t => t.Item1.Index == current.Index);
            int currentIdx = _indexMap[current.Index];
            return currentIdx - 1 >= 0;
        }
        private Variable<T> PreviousVariable(Variable<T> current)
        {
            //int currentIdx = Array.FindIndex(VariablesWithConstraints, t => t.Item1.Index == current.Index);
            int currentIdx = _indexMap[current.Index];
            return VariablesWithConstraints[currentIdx - 1].Item1;
        }
        private bool HasNextVariable(Variable<T> current)
        {
            //int currentIdx = Array.FindIndex(VariablesWithConstraints, t => t.Item1.Index == current.Index);
            int currentIdx = _indexMap[current.Index];
            return currentIdx + 1 <VariablesWithConstraints.Length;
        }
        private Variable<T> NextVariable(Variable<T> current)
        {
            //int currentIdx = Array.FindIndex(VariablesWithConstraints, t => t.Item1.Index == current.Index);
            int currentIdx = _indexMap[current.Index];
            return VariablesWithConstraints[currentIdx + 1].Item1;
        }

        //protected int[] SortConstraintwise()
        //{
        //    //var indexes = Enumerable.Range(0, Variables.Length).ToArray();     
        //    //var counts = Constraints.Select(l => l.Count).ToArray();
        //    //Array.Sort(counts, indexes);

        //    return indexes.Reverse().ToArray();


        //}
        private int AlredySetVariables()
        {
            int set = 0;
            for (int i = 0; i < VariablesWithConstraints.Length; i++)
            {
                if (VariablesWithConstraints[i].Item1.Value.Equals(VariablesWithConstraints[i].Item2.Default))
                    set++;
            }
            return set;
        }

       

        protected void SortDomainwise()
        {

            Array.Sort(VariablesWithConstraints, (t1,t2) => t1.Item2.Size.CompareTo(t2.Item2.Size));
            UpdateIndexMap();

        }
        protected void SortDomainwiseAndConstrainwise()
        {

            Array.Sort(VariablesWithConstraints, (t1, t2) => t1.Item2.Size.CompareTo(t2.Item2.Size)==0?
                                                            t2.Item3.Count.CompareTo(t1.Item3.Count):
                                                            t1.Item2.Size.CompareTo(t2.Item2.Size));

            UpdateIndexMap();

        }
        private void UpdateIndexMap()
        {
            for (int i = 0; i < _indexMap.Length; i++)
            {
                _indexMap[VariablesWithConstraints[i].Item1.Index] = i;
            }
        }

    }
}

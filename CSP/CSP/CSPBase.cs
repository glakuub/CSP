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
    abstract class CSPBase<T,S> where S: Variable<T>
    {

        public Tuple<S, Domain<T>, List<Constraint<T,S>>>[] VariablesWithConstraints;
        protected int[] _indexMap;
        public string FileSaveDirectory { set; get; }

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

                            
                            if(currentVariableDomain!=null && !currentVariableDomain.HasNext())
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
                if (currentVariableDomain.HasNext())
                {
                    while (currentVariableDomain.HasNext())
                    {
                        var currentDomainValue = currentVariableDomain.Next();
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
                else
                {
                    currentVariableDomain.Reset();
                    current.Value = currentVariableDomain.UnsetValue();
                    backtrack = true;
                }

                
                _iteration++;
            }

            _timeToComplete.Stop();
            if (printSolutions)
            {
                PrintSolutionsInfo();
            }
            
            

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

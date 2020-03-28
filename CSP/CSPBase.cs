using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSP
{
    class CSPBase<T,S> where S: Variable<T>
    {

        public Tuple<S, Domain<T>, List<Constraint<T,S>>>[] VariablesWithConstraints;
        protected List<S[]> foundSolutions;
        public void BacktrackingAlgorithm()
        {
            foundSolutions = new List<S[]>();
            Variable<T> current = null;
            bool hasSolution = false;
            bool backtrack = false;

            int iteration = 0;
            int visitedNodes = 0;
            int visitedNodesToFirst = 0;
            int foundSolutionsNumber = 0;
            

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
                            hasSolution = true;
                            //Console.WriteLine("found");
                            if (foundSolutionsNumber == 0)
                                visitedNodesToFirst = visitedNodes;
                            foundSolutionsNumber++;
                            foundSolutions.Add(SaveSolution());

                            
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

                
                currentVariableDomain = VariablesWithConstraints.Where(t => t.Item1.Index == current.Index).First().Item2;

                bool constraintsSatisfied = false;
                if (currentVariableDomain.HasNext())
                {
                    while (currentVariableDomain.HasNext())
                    {
                        var currentDomainValue = currentVariableDomain.Next();
                        current.Value = currentDomainValue;
                        visitedNodes++;
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

                
                iteration++;
            }

            Console.WriteLine($"visited nodes to first solution: {visitedNodesToFirst}");
            Console.WriteLine($"all visited nodes: {visitedNodes}");
            Console.WriteLine($"found solution: {hasSolution}");
            Console.WriteLine($"solutions number: {foundSolutionsNumber}");

            

        }

        private S[] SaveSolution()
        {
            var saved = new S[VariablesWithConstraints.Length];
            for(int i = 0; i<saved.Length;i++)
            {
                saved[i] = (S)Activator.CreateInstance(typeof(S),new object[] { VariablesWithConstraints[i].Item1 });
            }

            return saved;
        }
        private Variable<T> GoToLastWithNonemptyDomain(Variable<T> current)
        {
            var curr = current;
            while(HasPreviousVariable(curr))
            {
                var prev = PreviousVariable(current);
                var prevDomain = VariablesWithConstraints.Where(t => t.Item1.Index == prev.Index).First().Item2;
                if(prevDomain.RemainingSize()==0)
                {
                    break;
                }
                else
                {
                    curr = prev;
                }
            }
            return curr;
        }

        private Variable<T> StartVariable()
        {
            return VariablesWithConstraints[0].Item1;
        }
        private bool CheckConstraints(Variable<T> current)
        {
            bool result = true;
            int index = current.Index;
            var constraints = VariablesWithConstraints.Where(t => t.Item1.Index == index).First().Item3;
                foreach (var pred in constraints)
                {
                    if (!pred.Check(VariablesWithConstraints))
                    {
                        result = false;
                        break;
                    }
                }
            
            return result;
        }
        private bool HasPreviousVariable(Variable<T> current)
        {
            int currentIdx = Array.FindIndex(VariablesWithConstraints, t => t.Item1.Index == current.Index);
            return currentIdx - 1 >= 0;
        }
        private Variable<T> PreviousVariable(Variable<T> current)
        {
            int currentIdx = Array.FindIndex(VariablesWithConstraints, t => t.Item1.Index == current.Index);
            return VariablesWithConstraints[currentIdx - 1].Item1;
        }
        private bool HasNextVariable(Variable<T> current)
        {
            int currentIdx = Array.FindIndex(VariablesWithConstraints, t => t.Item1.Index == current.Index);
            return currentIdx + 1 <VariablesWithConstraints.Length;
        }
        private Variable<T> NextVariable(Variable<T> current)
        {
            int currentIdx = Array.FindIndex(VariablesWithConstraints, t => t.Item1.Index == current.Index);
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

        }

    }
}

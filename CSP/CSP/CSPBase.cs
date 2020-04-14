﻿using CSP.CSP.Heuristics.ValueSelection;
using CSP.CSP.Heuristics.VariableSelection;
using CSP.Util;
using System;
using System.Collections.Generic;
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
        public IValueSelectionHeuristics<T> ValueSelectionHeuristics { set; get; }
        public string FileSaveDirectory { set; get; }
        public ValueSelection ValueSelection { set; get; }

        protected List<S[]> _foundSolutions;
        protected IVariableSelectionHeuristics<T, S> _variableSelectionHeuristics;

        private Stopwatch _timeToFirst;
        private Stopwatch _timeToComplete;
        private int _iteration = 0;
        private int _visitedNodes = 0;
        private int _visitedNodesToFirst = 0;
        private int _foundSolutionsNumber = 0;
        private int _backtracksNumber;
        private int _backtracksNumberToFirstSolution;
        private bool _hasSolution = false;

       
        public virtual void BacktrackingAlgorithm(bool printSolutions = false)
        {
            int alreadySet = 0;

            _timeToFirst = new Stopwatch();
            _timeToComplete = new Stopwatch();
            _foundSolutions = new List<S[]>();
            S current = null;
            bool backtrack = false;

            _timeToFirst.Start();
            _timeToComplete.Start();
            List<Tuple<int, T>> nodes = new List<Tuple<int, T>>();

            bool[][] domainsState = new bool[VariablesWithConstraints.Length][];
            for(int i = 0; i < VariablesWithConstraints.Length; i++ )
            {
                domainsState[i] = new bool[VariablesWithConstraints[i].Item2.Size];
            }
            Domain<T> currentVariableDomain = null;
            bool[] currentVariableDomainState = null;

            while (true)
            {

                if (current == null)
                { 
                    current = _variableSelectionHeuristics.StartVariable();     
                }
                else 
                {
                    
                    if (backtrack)
                    {
                        if (_variableSelectionHeuristics.HasPrev())
                        {
                            current = _variableSelectionHeuristics.Prev();
                            backtrack = false;
                            
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        
                        if (_variableSelectionHeuristics.HasNext())
                        {
                            current = _variableSelectionHeuristics.Next();
                        }
                        else
                        {
                            _hasSolution = true;
                            //Console.WriteLine("found");

                            if (_foundSolutionsNumber == 0)
                            {
                                _timeToFirst.Stop();
                                _visitedNodesToFirst = _visitedNodes;
                                _backtracksNumberToFirstSolution = _backtracksNumber;
                            }

                            _foundSolutionsNumber++;
                            _foundSolutions.Add(SaveSolution());

                            if (currentVariableDomain != null && !ValueSelectionHeuristics.HasNext())
                            {
                                if (currentVariableDomainState != null)
                                    ArrayExtensions.Fill(ref currentVariableDomainState, false);
                                current.Value = currentVariableDomain.Default;
                                current = _variableSelectionHeuristics.Prev();
                                _backtracksNumber++;

                            }
                            Console.WriteLine();
                        }
                    }
                    
                }
                

                
                
                currentVariableDomain = VariablesWithConstraints[current.Index].Item2;
                currentVariableDomainState = domainsState[current.Index];
               
                // Send current domain to heuristics
                ValueSelectionHeuristics.RegisterDomain(current, currentVariableDomain, ref currentVariableDomainState);

                bool constraintsSatisfied = false;
                if (!ValueSelectionHeuristics.HasNext())
                {
                    ArrayExtensions.Fill(ref currentVariableDomainState, false);
                    current.Value = currentVariableDomain.Default;
                    backtrack = true;
                    _backtracksNumber++;

                }
                else
                {
                    while (ValueSelectionHeuristics.HasNext())
                    {
                        var currentDomainValue = ValueSelectionHeuristics.Next();
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
                        ArrayExtensions.Fill(ref currentVariableDomainState, false);
                        current.Value = currentVariableDomain.Default;
                        backtrack = true;
                        _backtracksNumber++;
                    }
                }

                //if (_iteration % 100000 == 0)
                //    Console.Out.WriteAsync($"{alreadySet} ");

                //if (alreadySet != AlredySetVariables())
                //{
                //    alreadySet = AlredySetVariables();

                //}
                //_iteration++;

                //for (int i = 0; i < 9; i++)
                //{
                //    for (int j = 0; j < 9; j++)
                //    {
                //        Console.Write($"{VariablesWithConstraints[i * 9 + j].Item1.Value} ");
                //    }
                //    Console.WriteLine();
                //}
                //Thread.Sleep(5);
                //Console.Clear();
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
            S current = null;
            bool backtrack = false;

            _timeToFirst.Start();
            _timeToComplete.Start();


            //var domainsStack = new Stack<Domain<T>[]>();
            
            //Domain<T>[] domains = new Domain<T>[VariablesWithConstraints.Length];
            //for (int i = 0; i < domains.Length; i++)
            //{ 
            //    domains[i] = new Domain<T>(VariablesWithConstraints[i].Item2);

            //}
            //domainsStack.Push(domains);
            

            var domainsStateStack = new Stack<bool[][]>();

            bool[][] domainsState = new bool[VariablesWithConstraints.Length][];
            for (int i = 0; i < VariablesWithConstraints.Length; i++)
            {
                domainsState[i] = new bool[VariablesWithConstraints[i].Item2.Size];
            }
            
            domainsStateStack.Push(domainsState);
            
            bool[][] currentDomainsState = domainsStateStack.Peek();

            Domain<T> currentVariableDomain = null;
            while (true)
            {
             
                if (current == null)
                {
                    current = _variableSelectionHeuristics.StartVariable();
                }
                else
                {
                    if (backtrack)
                    {

                        if (_variableSelectionHeuristics.HasPrev())
                        {
                            current = _variableSelectionHeuristics.Prev();
                            backtrack = false;
                            domainsStateStack.Pop();
                            currentDomainsState = domainsStateStack.Peek();
                            _backtracksNumber++;
                        }
                        else
                        {
                            break;
                        }

                    }
                    else
                    {
                        if(_variableSelectionHeuristics.HasNext())
                        {
                            current = _variableSelectionHeuristics.Next();
                        }
                        else
                        {
                            _hasSolution = true;
                            //Console.Out.WriteLineAsync("found");
                            if (_foundSolutionsNumber == 0)
                            {
                                _visitedNodesToFirst = _visitedNodes;
                                _timeToFirst.Stop();
                                _backtracksNumberToFirstSolution = _backtracksNumber;
                            }
                            _foundSolutionsNumber++;
                            _foundSolutions.Add(SaveSolution());
                            
                            domainsStateStack.Pop();
                            domainsStateStack.Pop();
                            currentDomainsState = domainsStateStack.Peek();
                            currentVariableDomain = VariablesWithConstraints[current.Index].Item2;
                            //currentVariableDomain.Reset();
                            current.Value = currentVariableDomain.UnsetValue();
                            current = _variableSelectionHeuristics.Prev();
                            _backtracksNumber++;
                            
                            
                        }
                    }
                }


                currentVariableDomain = VariablesWithConstraints[current.Index].Item2;
                ValueSelectionHeuristics.RegisterDomain(current, currentVariableDomain, ref currentDomainsState[current.Index]);


                bool constraintsSatisfied = false;
                if (!ValueSelectionHeuristics.HasNext())
                {
                    
                    current.Value = currentVariableDomain.UnsetValue();
                    backtrack = true;

                }
                else
                {
                    
                    while (ValueSelectionHeuristics.HasNext())
                    {
                        var currentDomainValue = ValueSelectionHeuristics.Next();
                        current.Value = currentDomainValue;
                        _visitedNodes++;

                       
                        bool[][] temp = new bool[currentDomainsState.Length][];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            temp[i] = new bool[currentDomainsState[i].Length];
                            currentDomainsState[i].CopyTo(temp[i], 0);
                          
                        }
                        domainsStateStack.Push(temp);

                        currentDomainsState = domainsStateStack.Peek();
                        currentVariableDomain = VariablesWithConstraints[current.Index].Item2;

                        if (!FilterOutDomains(current, ref currentDomainsState, _variableSelectionHeuristics))
                        {

                            constraintsSatisfied = true;
                            break;
                           
                        }

                        domainsStateStack.Pop();
                        currentDomainsState = domainsStateStack.Peek();
                        currentVariableDomain = VariablesWithConstraints[current.Index].Item2;

                    }
                    if (!constraintsSatisfied)
                    {
                       
                        current.Value = currentVariableDomain.UnsetValue();
                        backtrack = true;
                       
                    }
                }

                //if (_iteration % 100000 == 0)
                //    Console.Out.WriteAsync($"{alreadySet} ");

                //if (alreadySet != AlredySetVariables())
                //{
                //    alreadySet = AlredySetVariables();

                //}
                //_iteration++;

                //for (int i = 0; i < 9; i++)
                //{
                //    for (int j = 0; j < 9; j++)
                //    {
                //        Console.Write($"{VariablesWithConstraints[i * 9 + j].Item1.Value} ");
                //    }
                //    Console.WriteLine();
                //}
                //Thread.Sleep(5);
                //Console.Clear();

            }

            _timeToComplete.Stop();
            if (printSolutions)
            {
                PrintSolutionsInfo();
            }



        }

      

        protected virtual bool FilterOutDomains(S current, ref bool[][] currentDomainsState, IVariableSelectionHeuristics<T,S> variableSelectionHeuristics)
        {
            bool reducedToZero = false;
            
            var currentVariableConstraints = VariablesWithConstraints[current.Index].Item3;
            for (int i = 0; i < currentVariableConstraints.Count; i++)
            {
                var pairVariable = VariablesWithConstraints[currentVariableConstraints[i].Id2].Item1;
                
                var constraint = currentVariableConstraints[i];

                if (variableSelectionHeuristics.IsBefore(current,pairVariable))
                {
                    var pairVariableDomain = VariablesWithConstraints[pairVariable.Index].Item2;

                    var domainArray = pairVariableDomain.AsArray();
                    int domainSize = domainArray.Length;
                    for (int j = 0; j < domainSize; j++)
                    {

                        if (!constraint.Check(current.Value, domainArray[j]))
                        {
                            currentDomainsState[pairVariable.Index][j] = true;
                            domainSize--;
                        }

                    }
                    if (domainSize == 0)
                    { 
                        reducedToZero =  true;
                        break;
                    }

                   

                }

            }

            return reducedToZero;

        }
   
        private string CreateInfoString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"\nvisited nodes to first solution: {(_hasSolution?_visitedNodesToFirst.ToString():"")}");
            sb.AppendLine($"Backtracks to first solution: {_backtracksNumberToFirstSolution}");
            sb.AppendLine($"all visited nodes: {_visitedNodes}");
            sb.AppendLine($"all backtracks: {_backtracksNumber}");
            sb.AppendLine($"found solution: {_hasSolution}");
            sb.AppendLine($"solutions number: {_foundSolutionsNumber}");
            sb.AppendLine($"time to first solution: {(_hasSolution?_timeToFirst.Elapsed.ToString():" ")}");
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
        private bool CheckConstraints(Variable<T> current)
        {
            bool result = true;
            int index = current.Index;
            var constraints = VariablesWithConstraints[current.Index].Item3;
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
        private int AlredySetVariables()
        {
            int set = 0;
            for (int i = 0; i < VariablesWithConstraints.Length; i++)
            {
                if (!VariablesWithConstraints[i].Item1.Value.Equals(VariablesWithConstraints[i].Item2.Default))
                    set++;
            }
            return set;
        }
    }
}

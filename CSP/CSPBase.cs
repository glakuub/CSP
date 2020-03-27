using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CSP
{
    class CSPBase<T>
    {
        public Variable<T>[] Variables { set; get; }
        public Domain<T>[] Domains { set; get; }
        public List<Constraint<T>>[] Constraints { set; get; }
        public void BacktrackingAlgorithm()
        {
            int current = -1;
            bool finished = false;
            bool backtrack = false;

            int iteration = 0;
            while (!finished)
            {
                if (!backtrack)
                {
                    if (HasNextVariable(current))
                        current = NextVariable(current);
                    else
                        break;
                }
                else
                {
                    if (HasPreviousVariable(current))
                        current = PreviousVariable(current);
                    else
                        break;
                }

                backtrack = false;
                var currentVariableDomain = Domains[current];

                bool constraintsSatisfied = false;
                if (currentVariableDomain.HasNext())
                {
                    while (currentVariableDomain.HasNext())
                    {
                        var currentDomainValue = currentVariableDomain.Next();
                        Variables[current].Value = currentDomainValue;
                        if (CheckConstraints(current))
                        {
                            constraintsSatisfied = true;
                            break;
                        }
                    }
                    if (!constraintsSatisfied)
                    {
                        currentVariableDomain.Reset();
                        Variables[current].Value = currentVariableDomain.UnsetValue();
                        backtrack = true;
                    }
                }
                else
                {
                    currentVariableDomain.Reset();
                    Variables[current].Value = currentVariableDomain.UnsetValue();
                    backtrack = true;
                }


                Console.WriteLine($"iteration: {iteration}");
                iteration++;
            }
           

        }

        private bool CheckConstraints(int current)
        {
            bool result = true;
            if (current < Constraints.Length && Constraints[current] != null)
            {
                foreach (var pred in Constraints[current])
                {
                    if (!pred.Check(Variables))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }
        private bool HasPreviousVariable(int current)
        {
            var prev = current - 1;

            return prev >= 0;
        }
        private int PreviousVariable(int current)
        {
            var next = current - 1;

            return next;
        }
        private bool HasNextVariable(int current)
        {
            int next;
            if (current == -1)
                next = 0;
            else
            {
                next = current + 1;
            }
            return next < Variables.Length;
        }
        private int NextVariable(int current)
        {
            int next;
            if (current == -1)
                next = 0;
            else
            {
                next = current + 1;
            }
            return next;
        }
    }
}

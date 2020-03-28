﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSP
{
    class JolkaCSP: CSPBase<string,JolkaVariable>
    {
        private Jolka _jolka;
        private char EMPTY_CHAR;
        public JolkaCSP(Jolka jolka)
        {
            _jolka = jolka;
            EMPTY_CHAR = jolka.Empty;

            var variables = ParseVariables(jolka).ToArray();
            var domains = CreateDmains(jolka, variables);
            var constraints = CreateConstraints(jolka, variables);

            VariablesWithConstraints = new Tuple<JolkaVariable, Domain<string>, List<Constraint<string, JolkaVariable>>>[variables.Length];
            for(int i=0; i< variables.Length;i++)
            {
                VariablesWithConstraints[i] = new Tuple<JolkaVariable, Domain<string>, List<Constraint<string, JolkaVariable>>>(variables[i], domains[i], constraints[i]);
            }

            SortDomainwise();

        }

        public new void BacktrackingAlgorithm()
        {
            base.BacktrackingAlgorithm();
            foreach(var s in foundSolutions)
            {
                PrintOnBoard(s);
            }

                
        }

        public void PrintOnBoard(JolkaVariable[] solutions)
        {
            var board = _jolka.Board;
            foreach(var v in solutions)
            {
                var local = (v as JolkaVariable);
                 if(local.Orientation.Equals(Orientation.HORIZONTAL))
                     {
                            for(int i=0;i<local.Value.Length;i++)
                            {
                                board.SetAt(local.Start.Item1, local.Start.Item2 + i,local.Value[i]);
                            }
                     }
                 else
                 {
                    for (int i = 0; i < local.Value.Length; i++)
                    {
                        board.SetAt(local.Start.Item1 + i, local.Start.Item2, local.Value[i]);
                    }
                }
            }
            Console.WriteLine(board.ToString());
        }

    
        private Domain<string>[] CreateDmains(Jolka jolka, JolkaVariable[] variables)
        {
            var result = new Domain<string>[variables.Length];
            for(int i = 0; i<result.Length;i++)
            {
                var validWords = jolka.Words.Where(w=>w.Length==variables[i].Value.Length);
                result[i] = new Domain<string>(validWords.ToArray()) { Default = new string(EMPTY_CHAR, variables[i].Value.Length) };
            }

            return result;
        }
        private List<JolkaVariable> ParseVariables(Jolka jolka)
        {
            int varIndex = 0;
            var board = jolka.Board;
            var vars = new List<JolkaVariable>();
            for (int row = 0; row < board.Rows; row++)
            {
                int wordStart = 0;
                int wordLength = 0;
                for (int column = 0; column < board.Columns; column++)
                {


                    if(board.GetAt(row, column).Equals(jolka.Block))
                    {
                        if(wordLength == 0)
                        {
                            wordStart++;
                        }
                        else if(wordLength == 1)
                        {
                            wordStart = column + 1;
                            wordLength = 0;
                        }
                        else
                        {
                            vars.Add(new JolkaVariable()
                            {
                                Index = varIndex,
                                Value = new string(jolka.Empty, wordLength),
                                Start = new Tuple<int, int>(row, wordStart),
                                Orientation = Orientation.HORIZONTAL
                            }) ;
                            wordLength = 0;
                            wordStart = column + 1;
                            varIndex++;
                        }
                    }
                    else
                    {
                        wordLength++;
                    }
                       
                    
                }
                if (wordLength > 1)
                {
                    vars.Add(new JolkaVariable()
                    {
                        Index = varIndex,
                        Value = new string(jolka.Empty, wordLength),
                        Start = new Tuple<int, int>(row, wordStart),
                        Orientation = Orientation.HORIZONTAL
                    }) ;
                    varIndex++;
                }
            }
            for (int column = 0; column < board.Columns; column++)
            {
                int wordStart = 0;
                int wordLength = 0;
                for (int row = 0; row < board.Rows; row++)
                {

                    if(board.GetAt(row, column).Equals(jolka.Block))
                    {
                        if(wordLength == 0)
                        {
                            wordStart++;

                        }
                        else if(wordLength == 1)
                        {
                            wordStart = row + 1;
                            wordLength = 0;
                        }
                        else
                        {
                            vars.Add(new JolkaVariable()
                            {
                                Index = varIndex,
                                Value = new string(jolka.Empty, wordLength),
                                Start = new Tuple<int, int>(wordStart, column),
                                Orientation = Orientation.VERTICAL
                            }) ;
                            varIndex++;
                            wordLength = 0;
                            wordStart = row + 1;
                        }

                    }
                    else 
                    {
                        wordLength++;
                      
                    }

                   
                  
                }
                if (wordLength > 1)
                {
                    vars.Add(new JolkaVariable()
                    {
                        Index = varIndex,
                        Value = new string(jolka.Empty, wordLength),
                        Start = new Tuple<int, int>(wordStart, column),
                        Orientation = Orientation.VERTICAL
                    });
                    varIndex++;
                }
                }
            return vars;
        }

        private List<Constraint<string,JolkaVariable>>[] CreateConstraints(Jolka jolka, Variable<string>[] variables)
        {
            var constraints = new List<Constraint<string,JolkaVariable>>[variables.Length];

            for(int i = 0; i< variables.Length;i++)
            {
                constraints[i] = new List<Constraint<string, JolkaVariable>>();
                for(int j = 0; j< variables.Length;j++)
                {
                    if(i!=j)
                    {
                        var c = ConstraintOfIntersection(variables, i, j);
                        if(c!=null)
                        {
                            constraints[i].Add(c);
                        }
                    }
                    
                }
            }


            return constraints;
        }

        private Constraint<string, JolkaVariable> ConstraintOfIntersection(Variable<string>[] variables, int v1idx, int v2idx)
        {
            Constraint<string, JolkaVariable> constraint = null;
            
            var v1 = variables[v1idx] as JolkaVariable;
            var v1fields = VariableFields(v1);

            var v2 = variables[v2idx] as JolkaVariable;
            var v2fields = VariableFields(v2);

            var intersection = v1fields.Intersect(v2fields).ToArray();

            if(intersection!=null && intersection.Length > 0)
            {
                int var1intPosition = v1fields.IndexOf(intersection[0]);
                int var2intPosition = v2fields.IndexOf(intersection[0]);
                constraint = new Constraint<string, JolkaVariable>(v1idx, v2idx, (s1, s2) => (s1[var1intPosition].Equals(s2[var2intPosition]) 
                                                                            || s2[var2intPosition].Equals(EMPTY_CHAR)));
            }

            return constraint;
        }

        private List<Tuple<int,int>> VariableFields(JolkaVariable variable)
        {
            var fields = new List<Tuple<int, int>>();
            if (variable.Orientation.Equals(Orientation.HORIZONTAL))
            {
                for (int i = variable.Start.Item2; i < variable.Start.Item2 + variable.Value.Length; i++)
                {
                    fields.Add(new Tuple<int, int>(variable.Start.Item1, i));
                }

            }
            else
            {
                for (int i = variable.Start.Item1; i < variable.Start.Item1 + variable.Value.Length; i++)
                {
                    fields.Add(new Tuple<int, int>(i, variable.Start.Item2));
                }
            }
            return fields;
        }

    }
}

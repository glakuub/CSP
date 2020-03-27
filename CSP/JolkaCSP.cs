using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CSP
{
    class JolkaCSP: CSPBase<string>
    {
        private Jolka jolka;
        
        public JolkaCSP(Jolka jolka)
        {
            this.jolka = jolka;
            Variables = ParseVariables(jolka).ToArray();
            //foreach(var v in Variables)
            //{
            //    Console.WriteLine(v);
            //}
            Domains = CreateDmains(jolka, Variables.Length);
            Constraints = CreateConstraints(jolka, Variables);
            //Constraints = new List<Constraint<string>>[0]; //CreateConstraints(jolka, Variables);
            
        }


        public void PrintOnBoard()
        {
            var board = jolka.Board;
            foreach(var v in Variables)
            {
                var local = (v as JolkaVariable);
                 if(local.orientation.Equals(Orientation.HORIZONTAL))
                     {
                            for(int i=0;i<v.Value.Length;i++)
                            {
                                board.SetAt(local.Start.Item1, local.Start.Item2 + i,local.Value[i]);
                            }
                     }
                 else
                 {
                    for (int i = 0; i < v.Value.Length; i++)
                    {
                        board.SetAt(local.Start.Item1 + i, local.Start.Item2, local.Value[i]);
                    }
                }
            }
            Console.WriteLine(board.ToString());
        }

        public void BacktrackingAlgorithm()
        {
            base.BacktrackingAlgorithm();
            
        }

        private Domain<string>[] CreateDmains(Jolka jolka, int variablesCount)
        {
            var result = new Domain<string>[variablesCount];
            for(int i = 0; i<variablesCount;i++)
            {
                var validWords = jolka.Words.Where(w=>w.Length==Variables[i].Value.Length);
                result[i] = new Domain<string>(validWords.ToArray()) { Default = new string('_',Variables[i].Value.Length) };
            }

            return result;
        }
        private List<JolkaVariable> ParseVariables(Jolka jolka)
        {
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
                                Value = new string(jolka.Empty, wordLength),
                                Start = new Tuple<int, int>(row, wordStart),
                                orientation = Orientation.HORIZONTAL
                            });
                            wordLength = 0;
                            wordStart = column + 1;
                        }
                    }
                    else
                    {
                        wordLength++;
                    }
                       
                    
                }
                if (wordLength > 1)
                    vars.Add(new JolkaVariable() { Value = new string(jolka.Empty, wordLength),
                        Start = new Tuple<int, int>(row, wordStart),
                        orientation = Orientation.HORIZONTAL
                    });

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
                                Value = new string(jolka.Empty, wordLength),
                                Start = new Tuple<int, int>(wordStart, column),
                                orientation = Orientation.VERTICAL
                            });

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
                    vars.Add(new JolkaVariable() { Value = new string(jolka.Empty, wordLength), 
                        Start = new Tuple<int, int>(wordStart, column), 
                        orientation = Orientation.VERTICAL });
            }
            return vars;
        }

        private List<Constraint<string>>[] CreateConstraints(Jolka jolka, Variable<string>[] variables)
        {
            var constraints = new List<Constraint<string>>[variables.Length];

            for(int i = 0; i< variables.Length;i++)
            {
                constraints[i] = new List<Constraint<string>>();
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

        private Constraint<string> ConstraintOfIntersection(Variable<string>[] variables, int v1idx, int v2idx)
        {
            Constraint<string> constraint = null;
            
            var v1 = variables[v1idx] as JolkaVariable;
            var v1fields = VariableFields(v1);

            var v2 = variables[v2idx] as JolkaVariable;
            var v2fields = VariableFields(v2);

            var intersection = v1fields.Intersect(v2fields).ToArray();

            if(intersection!=null && intersection.Length > 0)
            {
                int var1intPosition = v1fields.IndexOf(intersection[0]);
                int var2intPosition = v2fields.IndexOf(intersection[0]);
                constraint = new Constraint<string>(v1idx, v2idx, (s1, s2) => (s1[var1intPosition].Equals(s2[var2intPosition]) 
                                                                            || s2[var2intPosition].Equals('_')));
            }

            return constraint;
        }

        private List<Tuple<int,int>> VariableFields(JolkaVariable variable)
        {
            var fields = new List<Tuple<int, int>>();
            if (variable.orientation.Equals(Orientation.HORIZONTAL))
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CSP
{
    class SudokuCSP:CSPBase<char,Variable<char>>
    {
        //Sudoku Sudoku {set;get;}
        private int _rows;
        private int _columns;
        private char EMPTY;

        public SudokuCSP(Sudoku board)
        {
            _rows = board.Board.Rows;
            _columns = board.Board.Columns;
            //Sudoku = board;
            EMPTY = board.Empty;
            var vars = ParseVariables(board);
            var doms = CreateDomains(vars);
            var cons = CreateConstraints(_rows, _columns);

            VariablesWithConstraints = new Tuple<Variable<char>, Domain<char>, List<Constraint<char, Variable<char>>>>[vars.Length];
            for(int i = 0; i<vars.Length;i++)
            {
                VariablesWithConstraints[i] = new Tuple<Variable<char>, Domain<char>, List<Constraint<char, Variable<char>>>>(vars[i], doms[i], cons[i]);
            }

        }
        public new void BacktrackingAlgorithm(bool printSolutions = false)
        {
            base.BacktrackingAlgorithm();
            if (printSolutions)
            {
                foreach (var v in foundSolutions)
                {
                    PrintOnBoard(v);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
            
        }

        private void PrintOnBoard(Variable<char>[] variables)
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns; column++)
                {
                    Console.Write($"{variables[row * _columns + column].Value} ");
                }
                Console.WriteLine();
            }

        }
        private Variable<char>[] ParseVariables(Sudoku sudoku)
        {
            var variables = new Variable<char>[sudoku.Board.Rows * sudoku.Board.Columns];
            for(int row = 0; row < sudoku.Board.Rows; row++)
            {
                for(int column = 0; column < sudoku.Board.Columns;column++)
                {
                    var varIndex = row * sudoku.Board.Columns + column;
                    if (sudoku.Board.GetAt(row, column) == 0)
                    {
                        variables[varIndex] = new Variable<char>() { Index = varIndex, Value = EMPTY };
                    }
                    else
                    {
                        variables[varIndex] = new Variable<char>() { Index = varIndex, Value = (char)(sudoku.Board.GetAt(row,column) + 48) };
                    }

                }
            }

            return variables;
        }
        private Domain<char>[] CreateDomains(Variable<char>[] variables)
        {
            var domains = new Domain<char>[variables.Length];
            for(int i = 0; i< domains.Length;i++)
            {
                if(variables[i].Value == EMPTY)
                {
                    domains[i] = new Domain<char>("123456789".ToCharArray()) { Default = EMPTY };
                }
                else
                {
                    domains[i] = new Domain<char>(new char[1] { variables[i].Value }) { Default = variables[i].Value };
                }
            }

            return domains;
        }

        public List<Constraint<char,Variable<char>>>[] CreateConstraints(int rows, int columns)
        {
            var constraints = new List<Constraint<char, Variable<char>>>[rows * columns];
            for(int row =0; row<rows;row++)
            {
                for(int column = 0; column < columns; column++)
                {
                    constraints[row * columns + column] = GenerateSudokuConstraints(row, column, rows, columns);
                }
            }

            return constraints;
        }
            




        public static List<Constraint<char, Variable<char>>> GenerateSudokuConstraints(int row, int column, int maxRow, int maxColumn)
        {
            var result = new List<Constraint<char, Variable<char>>>();
            for (int i = 0; i < maxRow; i++)
            {
                if (i != row)
                {
                    result.Add(new Constraint<char, Variable<char>>(row * maxRow + column, i * maxRow + column, (f1, f2) => f1 != f2));
                }
            }
            for (int i = 0; i < maxRow; i++)
            {
                if (i != column)
                {
                    result.Add(new Constraint<char, Variable<char>>(row * maxRow + column, row * maxRow + i, (f1, f2) => f1 != f2));
                }
            }
            List<Tuple<int, int>> neighbours = new List<Tuple<int, int>>();
            var ralativeRow = row % 3;
            var relativeColumn = column % 3;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (!(i == ralativeRow && j == relativeColumn))
                    {
                        neighbours.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            neighbours = neighbours.Select(t => new Tuple<int, int>(t.Item1 + ((int)row / 3) * 3, t.Item2 + ((int)column / 3) * 3)).ToList();
            foreach (var n in neighbours)
            {
                result.Add(new Constraint<char, Variable<char>>(row * maxRow + column, n.Item1 * maxRow + n.Item2, (f1, f2) => f1 != f2));
            }
            return result;
        }
    }
}

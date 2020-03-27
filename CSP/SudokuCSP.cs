using System;
using System.Collections.Generic;
using System.Threading;

namespace CSP
{
    class SudokuCSP:CSPBase<int>
    {
        Sudoku Sudoku {set;get;}
        private int _rows;
        private int _columns;

       

        //Variable<int>[,] Variables;
        //public Domain<int>[,] Domains { set; get; }
        //public List<Constraint<int>>[,] Constraints { set; get; }

        public SudokuCSP(Sudoku board)
        {
            _rows = board.Board.Rows;
            _columns = board.Board.Columns;
            Sudoku = board;
            Variables = new Variable<int>[_rows * _columns];
            for(int row = 0; row < _rows;row++)
            {
                for(int column = 0; column < _columns; column++)
                {
                    Variables[row * _columns +  column] = new Variable<int>() { Value = Sudoku.Board.GetAt(row, column) };
                }
            }
            Constraints = new List<Constraint<int>>[_rows * _columns];
        }

        //public void BacktrackingAlgorithm()
        //{
        //    Tuple<int,int> current = null;
        //    bool finished = false;
        //    bool backtrack = false;

        //    while (!finished)
        //    {
        //        if (!backtrack)
        //        {
        //            if (HasNextVariable(current))
        //                current = NextVariable(current);
        //            else
        //                break;
        //        }
        //        else
        //        {
        //            if (HasPreviousVariable(current))
        //                current = PreviousVariable(current);
        //            else
        //                break;
        //        }

        //        backtrack = false;
        //        var currentVariableDomain = Domains[current.Item1, current.Item2];

        //        bool constraintsSatisfied = false;
        //        if (currentVariableDomain.HasNext())
        //        {
        //            while (currentVariableDomain.HasNext())
        //            {
        //                var currentDomainValue = currentVariableDomain.Next();
        //                Variables[current.Item1, current.Item2].Value = currentDomainValue;
        //                if (CheckConstraints(current))
        //                {
        //                    constraintsSatisfied = true;
        //                    break;
        //                }
        //            }
        //            if (!constraintsSatisfied)
        //            {
        //                currentVariableDomain.Reset();
        //                Variables[current.Item1, current.Item2].Value = currentVariableDomain.Default();
        //                backtrack = true;
        //            }
        //        }
        //        else
        //        {
        //            currentVariableDomain.Reset();
        //            Variables[current.Item1, current.Item2].Value = currentVariableDomain.Default();
        //            backtrack = true;
        //        }
        //        //Console.Clear();
        //        //for (int i = 0; i < _rows; i++)
        //        //{
        //        //    for (int j = 0; j < _columns; j++)
        //        //    {
        //        //        Console.Write($"{Variables[i, j]} ");
        //        //    }
        //        //    Console.WriteLine();
        //        //}
        //        //Thread.Sleep(10);


        //    }
        //    Console.Clear();
        //    for (int i = 0; i < _rows; i++)
        //    {
        //        for (int j = 0; j < _columns; j++)
        //        {
        //            Console.Write($"{Variables[i, j]} ");
        //        }
        //        Console.WriteLine();
        //    }
        //    Thread.Sleep(500);



        //}
        //private bool CheckConstraints(Tuple<int, int> current)
        //{
        //    bool result = true;
        //    if (Constraints[current.Item1, current.Item2] != null)
        //    {
        //        foreach (var pred in Constraints[current.Item1, current.Item2])
        //        {
        //            if (!pred.Check(Variables))
        //            {
        //                result = false;
        //                break;
        //            }
        //        }
        //    }
        //    return result;
        //}

        //private bool HasPreviousVariable(Tuple<int, int> current)
        //{
        //    var next = new Tuple<int, int>((current.Item1 * _columns + current.Item2 -1) / _rows,
        //                                  (current.Item1 * _columns + current.Item2 - 1) % _rows);

        //    return next.Item1 >= 0 && next.Item2 >= 0;
        //}
        //private Tuple<int, int> PreviousVariable(Tuple<int, int> current)
        //{
        //    var next = new Tuple<int, int>((current.Item1 * _columns + current.Item2 - 1) / _rows,
        //                                  (current.Item1 * _columns + current.Item2 - 1) % _rows);

        //    return next;
        //}
        //private bool HasNextVariable(Tuple<int,int> current)
        //{
        //    Tuple<int, int> next;
        //    if (current == null)
        //        next = new Tuple<int, int>(0, 0);
        //    else
        //    {
        //        next = new Tuple<int, int>((current.Item1 * _columns + current.Item2 + 1) / _rows,
        //                                      (current.Item1 * _columns + current.Item2 + 1) % _rows);
        //    }
        //    return next.Item1 < _rows && next.Item2 < _columns;
        //}
        //private Tuple<int,int> NextVariable(Tuple<int,int> current)
        //{
        //    Tuple<int, int> next;
        //    if (current == null)
        //        next = new Tuple<int, int>(0, 0);
        //    else
        //    {
        //        next = new Tuple<int, int>((current.Item1 * _columns + current.Item2 + 1) / _rows,
        //                                  (current.Item1 * _columns + current.Item2 + 1) % _rows);
        //    }
        //    return next;
        //}

      

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSP
{
    class Program
    {
        private static string DATA_DIRECTORY_NAME = "ai-lab2-2020-dane";
        private static string DIRECTORY = $@"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName}\{DATA_DIRECTORY_NAME}/Jolka";
        private static string FILE_NAME = "Sudoku.csv";
        private static string FILE_PATH = $@"{DIRECTORY}\{FILE_NAME}";
        private const int ROWS = 9;
        private const int COLUMNS = 9;


        static void Main(string[] args)
        {

            var jolka = Loader.LoadJolka($@"{DIRECTORY}\puzzle2", $@"{DIRECTORY}\words2", '#','_');
            var jolkaCSP = new JolkaCSP(jolka);
            jolkaCSP.BacktrackingAlgorithm();
            jolkaCSP.PrintOnBoard();
            //Console.WriteLine(jolka);


            //var sbs = Loader.LoadSudokuBoards(FILE_PATH);

            //var domains = new Domain<int>[9 * 9];
            //for (int row = 0; row < ROWS; row++)
            //{

            //    for (int column = 0; column < COLUMNS; column++)
            //    {
            //        if(sbs[0].Board.GetAt(row,column)==0)
            //            domains[row * COLUMNS + column] = new Domain<int>(new int[] {1,2,3,4,5,6,7,8,9});
            //        else
            //            domains[row * COLUMNS + column] = new Domain<int>(new int[] { sbs[0].Board.GetAt(row,column) });
            //    }
            //}

            //var constraints = new List<Constraint<int>>[9 * 9];

            //for (int row = 0; row < ROWS; row++)
            //{

            //    for (int column = 0; column < COLUMNS; column++)
            //    {
            //        constraints[row * COLUMNS + column] = GenerateSudokuConstraints(row,column);

            //    }
            //}


            //var scsp = new SudokuCSP(sbs[0]);
            //scsp.Domains = domains;
            //scsp.Constraints = constraints;
            //scsp.BacktrackingAlgorithm();
        }

        public static List<Constraint<int>> GenerateSudokuConstraints(int row, int column)
        {
            var result = new List<Constraint<int>>();
            for(int i=0;i<ROWS;i++)
            {
                if(i!=row)
                {
                result.Add(new Constraint<int>(row * COLUMNS + column, i * COLUMNS + column, (f1, f2) => f1 != f2));
                }
            }
            for (int i = 0; i < COLUMNS; i++)
            {
                if (i != column)
                {
                    result.Add(new Constraint<int>(row * COLUMNS + column, row * COLUMNS + i  , (f1, f2) => f1 != f2));
                }
            }
            List<Tuple<int, int>> neighbours = new List<Tuple<int, int>>();
            var ralativeRow = row % 3;
            var relativeColumn = column % 3;
            for(int i = 0; i < 3;i++)
            {
                for(int j = 0; j < 3 ;j++)
                {
                    if (!(i == ralativeRow && j== relativeColumn))
                    {
                        neighbours.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            neighbours =  neighbours.Select(t => new Tuple<int, int>(t.Item1 + ((int)row / 3)*3, t.Item2 + ((int)column / 3)*3)).ToList();
            foreach(var n in neighbours)
            {
                result.Add(new Constraint<int>(row * COLUMNS + column, n.Item1 * COLUMNS + n.Item2, (f1, f2) => f1 != f2));
            }
            return result;
        }

        
    }
}

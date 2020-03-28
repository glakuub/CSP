using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSP
{
    class Program
    {
        private static string DATA_DIRECTORY_NAME = "ai-lab2-2020-dane";
        private static string SUDOKU_DIRECTORY = $@"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName}\{DATA_DIRECTORY_NAME}/";
        private static string JOLKA_DIRECTORY = $@"{SUDOKU_DIRECTORY}\Jolka";
        private static string FILE_NAME = "Sudoku.csv";
        private static string FILE_PATH = $@"{SUDOKU_DIRECTORY}\{FILE_NAME}";
       


        static void Main(string[] args)
        {


            //var sbs = Loader.LoadSudokuBoards(FILE_PATH,'_');
            //foreach (var s in sbs.TakeLast(6).Take(3))
            //{
            //    var scsp = new SudokuCSP(s);
            //    scsp.BacktrackingAlgorithm();
            //}
            var jolka = Loader.LoadJolka($@"{JOLKA_DIRECTORY}\puzzle1", $@"{JOLKA_DIRECTORY}\words1", '#', '_');
            var jolkaCSP = new JolkaCSP(jolka);
            jolkaCSP.BacktrackingAlgorithm();

        }




    }
}

using CSP.CSP;
using CSP.CSP.Heuristics.ValueSelection;
using CSP.CSP.Heuristics.VariableSelection;
using CSP.Util;
using System;
using System.IO;

namespace CSP
{
    class Program
    {
        private static string DATA_DIRECTORY_NAME = "ai-lab2-2020-dane";
        private static string SUDOKU_DIRECTORY = $@"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName}\{DATA_DIRECTORY_NAME}\";
        private static string SUDOKU_SOLUTIONS_DIRECTORY = $@"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName}\{DATA_DIRECTORY_NAME}\Solutions\";
        private static string FILE_NAME = "Sudoku.csv";
        private static string FILE_PATH = $@"{SUDOKU_DIRECTORY}\{FILE_NAME}";
        private static int SUDOKU_NUMBER = 22;
        private static int ITERATIONS = 10;
       


        static void Main(string[] args)
        {


            var sbs = Loader.LoadSudokuBoards(FILE_PATH, '_');
            for (int i = 0; i < ITERATIONS; i++)
            {
                var sudoku = new SudokuCSP(sbs[SUDOKU_NUMBER - 1], new MostConstrainedVariableSelection<char, Variable<char>>())
                {
                    FileSaveDirectory = SUDOKU_SOLUTIONS_DIRECTORY,
                    ValueSelectionHeuristics = new RandomOrderValueSelection<char>()

                };
                sudoku.BacktrackingAlgorithmForwardCheck(true);
            }

            //var sudoku = new SudokuCSP(sbs[SUDOKU_NUMBER - 1], new DefinitionOrderVariableSelection<char, Variable<char>>())
            //{
            //    FileSaveDirectory = SUDOKU_SOLUTIONS_DIRECTORY,
            //    ValueSelectionHeuristics = new DefinitionOrderValueSelection<char>()

            //};
            //sudoku.BacktrackingAlgorithm(true);

            //var sudokufc = new SudokuCSP(sbs[SUDOKU_NUMBER - 1], new DefinitionOrderVariableSelection<char, Variable<char>>())
            //{
            //    FileSaveDirectory = SUDOKU_SOLUTIONS_DIRECTORY,
            //    ValueSelectionHeuristics = new DefinitionOrderValueSelection<char>()

            //};
            //sudokufc.BacktrackingAlgorithmForwardCheck(true);


        }




    }
}

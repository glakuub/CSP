using CSP.CSP;
using CSP.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSP
{
    class Program
    {
        private static string DATA_DIRECTORY_NAME = "ai-lab2-2020-dane";
        private static string SUDOKU_DIRECTORY = $@"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName}\{DATA_DIRECTORY_NAME}\";
        private static string SUDOKU_SOLUTIONS_DIRECTORY = $@"{Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName}\{DATA_DIRECTORY_NAME}\Solutions\";
        private static string JOLKA_DIRECTORY = $@"{SUDOKU_DIRECTORY}\Jolka";
        private static string JOLKA_SOLUTIONS_DIRECTORY = $@"{SUDOKU_DIRECTORY}\Jolka\Solutions";
        private static string FILE_NAME = "Sudoku.csv";
        private static string FILE_PATH = $@"{SUDOKU_DIRECTORY}\{FILE_NAME}";
       


        static void Main(string[] args)
        {

            //Ładownie danych i urchomienie algorytmu dla trzech plików sudoku:
            //      1 rozwązanie
            //      wiele rozwiązań
            //      brak rozwiązań
            //Pliki z rezultatem działania algorytmu znajdują się w katalogu \CSP\ai-lab2-2020-dane\Solutions\

            var sbs = Loader.LoadSudokuBoards(FILE_PATH, '_');

            //var sudoku1Solution = new SudokuCSP(sbs[40]) { FileSaveDirectory = SUDOKU_SOLUTIONS_DIRECTORY };
            //sudoku1Solution.BacktrackingAlgorithm(true);

            var sudokufc = new SudokuCSP(sbs[10]) { FileSaveDirectory = SUDOKU_SOLUTIONS_DIRECTORY };
            sudokufc.BacktrackingAlgorithmForwardCheck(true);

            //var sudokuManySolutions = new SudokuCSP(sbs[42]) { FileSaveDirectory = SUDOKU_SOLUTIONS_DIRECTORY };
            //sudokuManySolutions.BacktrackingAlgorithm();

            //var sudoku0Solutions = new SudokuCSP(sbs[45]) { FileSaveDirectory = SUDOKU_SOLUTIONS_DIRECTORY };
            //sudoku0Solutions.BacktrackingAlgorithm();


            //Ładownie danych i urchomienie algorytmu dla dwóch plików Jolka:
            //      1 rozwązanie
            //      2 rozwiązania
            //Pliki z rezultatem działania algorytmu znajdują się w katalogu \CSP\ai-lab2-2020-dane\Jolka\Solutions\

            //var jolka1Solution = Loader.LoadJolka($@"{JOLKA_DIRECTORY}\puzzle4", $@"{JOLKA_DIRECTORY}\words4", '#', '_');
            //var jolkaCSP1Solution = new JolkaCSP(jolka1Solution) { FileSaveDirectory = JOLKA_SOLUTIONS_DIRECTORY };
            //jolkaCSP1Solution.BacktrackingAlgorithm();

            //var jolka2Solutions = Loader.LoadJolka($@"{JOLKA_DIRECTORY}\puzzle1", $@"{JOLKA_DIRECTORY}\words1", '#', '_');
            //var jolkaCSP2Solutions = new JolkaCSP(jolka2Solutions) { FileSaveDirectory = JOLKA_SOLUTIONS_DIRECTORY };
            //jolkaCSP2Solutions.BacktrackingAlgorithm();

            //var j1 = Loader.LoadJolka($@"{JOLKA_DIRECTORY}\puzzle0", $@"{JOLKA_DIRECTORY}\words0", '#', '_');
            //var j1cspfc = new JolkaCSP(j1) { FileSaveDirectory = JOLKA_SOLUTIONS_DIRECTORY, ValueSelection = ValueSelection.LEAST_CONSTRAINING };
            //j1cspfc.BacktrackingAlgorithmForwardCheck(true);

            //var j1csp = new JolkaCSP(j1) { FileSaveDirectory = JOLKA_SOLUTIONS_DIRECTORY, ValueSelection = ValueSelection.LEAST_CONSTRAINING };
            //j1csp.BacktrackingAlgorithm(true);

            //var j2 = Loader.LoadJolka($@"{JOLKA_DIRECTORY}\puzzle2", $@"{JOLKA_DIRECTORY}\words2", '#', '_');
            //var j2csp = new JolkaCSP(j2) { FileSaveDirectory = JOLKA_SOLUTIONS_DIRECTORY, ValueSelection = ValueSelection.DEFINITION };
            //j2csp.BacktrackingAlgorithm(true);

        }




    }
}

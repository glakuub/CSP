using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CSP
{
    class Loader
    {
        private const string SEPARATOR = ";";
        private const char EMPTY = '.';
        public static List<Sudoku> LoadSudokuBoards(string fileName, char empty, string separator = SEPARATOR)
        {
            var boards = new List<Sudoku>();
            using(var sr = new StreamReader(fileName))
            {
                string line;
                while((line = sr.ReadLine())!=null)
                {
                    var columns = line.Split(separator);
                    if (!columns[0].Equals("id"))
                    {
                        Sudoku board = new Sudoku();
                        board.Id = int.Parse(columns[0]);
                        board.Difficulty = double.Parse(columns[1]);
                        board.Board = ParseSudokuBoard(columns[2]);
                        board.Soulution = ParseSudokuBoard(columns[3]);
                        board.Empty = empty;
                        boards.Add(board);
                    }
                }
            }

            return boards;
        }

        public static Jolka LoadJolka(string boarFileName, string wordsFileName, char block, char empty)
        {
            Jolka result = null;
            if (File.Exists(boarFileName))
            {
                var matrix = ParseJolkaBoard(File.ReadAllLines(boarFileName));

                if (File.Exists(wordsFileName))
                {
                    var words = File.ReadAllLines(wordsFileName);

                    result = new Jolka()
                    {
                        Board = matrix,
                        Words = words,
                        Block = block,
                        Empty = empty
                    };
                }

            }
            return result;
        }

        private static Matrix<int> ParseSudokuBoard(string board, char empty=EMPTY)
        {
            int size = (int)Math.Sqrt(board.Length);
            var result = new Matrix<int>(size, size);
            for(int row = 0; row < size; row++)
            {
                for(int column = 0; column < size; column++)
                {
                    char charValue = board[row * size + column];
                    int toSet = 0;
                    if (!charValue.Equals(empty))
                        toSet = (int)char.GetNumericValue(charValue);
                    result.SetAt(row, column, toSet);
                }
            }
            return result;
        }

        private static Matrix<char> ParseJolkaBoard(string[] lines)
        {
            Matrix<char> result = null;
            if (lines != null)
            {
                int width = lines[0].Length;
                result = new Matrix<char>(lines.Length, width);
                for(int row = 0;row < lines.Length;row++)
                {
                    for(int column = 0; column < width;column++)
                    {
                        result.SetAt(row, column, lines[row][column]);
                    }
                }

            }
            return result;
        }
       
    }
}

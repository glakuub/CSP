using CSP.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.Games
{
    class Sudoku
    {
        public int Id { set; get; }
        public double Difficulty { set; get; }
        public char Empty { set; get; }
        public Matrix<int> Board { set; get; }
        public Matrix<int> Soulution { set; get; }

        public Sudoku DeepCopy()
        {
            Sudoku result = new Sudoku();
            result.Id = this.Id;
            result.Difficulty = this.Difficulty;
            result.Empty = this.Empty;
            result.Board = this.Board.DeepCopy();
            result.Soulution = this.Soulution.DeepCopy();
            return result;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"id: {Id}");
            sb.AppendLine($"difficulty: {Difficulty}");
            sb.AppendLine($"board:\n{Board}");
            sb.AppendLine($"solution:\n{Soulution}");
            return sb.ToString();
        }
    }
}

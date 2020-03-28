using System;
using System.Collections.Generic;
using System.Text;

namespace CSP.Util
{
    class Matrix<T>
    {
        private T[,] _matrix;
        public int Rows { private set; get; }
        public int Columns { private set; get; }  

        public Matrix(int rows, int columns)
        {
            this.Rows = rows;
            this.Columns = columns;
            _matrix = new T[rows, columns];
        }

        public void SetAt(int row, int column, T element)
        {
            _matrix[row, column] = element;
        }
        public T GetAt(int row, int column)
        {
            return _matrix[row, column];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for(int row = 0; row < Rows; row++)
            {
                for(int column = 0; column < Columns; column++)
                {
                    sb.Append($"{_matrix[row, column]} ");
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }
    }
}

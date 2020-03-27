using System;
using System.Collections.Generic;
using System.Text;

namespace CSP
{
    class Jolka
    {
        public string[] Words { set; get; }
        public Matrix<char> Board { set; get; }

        public char Block { set; get; }
        public char Empty { set; get; }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Board.ToString());
            sb.AppendLine("Words:");
            foreach(var w in Words)
            {
                sb.AppendLine(w);
            }
            return sb.ToString();
        }
    }
}

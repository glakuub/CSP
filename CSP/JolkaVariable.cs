using System;
using System.Collections.Generic;
using System.Text;

namespace CSP
{
    public enum Orientation { HORIZONTAL, VERTICAL}
    class JolkaVariable: Variable<string>
    {
        public Tuple<int,int> Start { set; get; }
        public Orientation Orientation;
        public JolkaVariable() { }
        public JolkaVariable(JolkaVariable src):base(src)
        {
            Start = new Tuple<int, int>(src.Start.Item1, src.Start.Item2);
            Orientation = src.Orientation;
        }
    }
}

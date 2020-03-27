using System;
using System.Collections.Generic;
using System.Text;

namespace CSP
{
    public enum Orientation { HORIZONTAL, VERTICAL}
    class JolkaVariable: Variable<string>
    {
        public Tuple<int,int> Start { set; get; }
        public Orientation orientation;

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Util
{
    class Logger
    {
        

       public string FileName { private set; get; }

        public Logger(string fileName)
        {
            FileName = fileName;
        }
        public void WriteLine(string line)
        {
            using (var file = new System.IO.StreamWriter(FileName,true))
            {
                file.WriteLine(line);
            }
        }

        public void AppendToLine(string line, int lineNumber)
        {
            if (!File.Exists(FileName))
            {
                WriteLine(line);

            }
            else
            {
                var lines = File.ReadAllLines(FileName);
                if (lines.Length - 1 < lineNumber)
                {
                    WriteLine(line);
                }
                else
                {
                    lines[lineNumber] = $"{lines[lineNumber]}, ,{line}";

                    File.WriteAllLines(FileName, lines);
                }
            }
        }

        
        

        
    }
}

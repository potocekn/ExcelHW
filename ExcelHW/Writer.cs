using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class Writer
    {
        public static void WriteDocument(string outputFile, List<List<Cell>> document)
        {
            StreamWriter output = new StreamWriter(outputFile);
            bool firstWord = true;
            for (int i = 0; i < document.Count; i++)
            {
                firstWord = true;
                for (int j = 0; j < document[i].Count; j++)
                {
                    if (firstWord)
                        firstWord = false;
                    else
                        output.Write(' ');

                    output.Write(document[i][j].ToString());
                }
                output.Write(Environment.NewLine);
            }
            output.Close();
        }
    }
}

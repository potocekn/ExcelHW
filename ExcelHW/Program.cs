using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class Program
    {
        public static void Run(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Argument Error");
            }
            else
            {
                try
                {
                    Processor processor = new Processor(Reader.ReadSheet(args[0]));
                    processor.CalculateValues();

                    Writer.WriteDocument(args[1], processor.document);
                }
                catch (Exception e)
                {
                    Console.WriteLine("File Error");
                }

            }
        }
        static void Main(string[] args)
        {
            Run(args);
        }
    }
}

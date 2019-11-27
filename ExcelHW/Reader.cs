using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class Reader
    {
        public static List<List<Cell>> ReadSheet(string fileName)
        {
            List<List<Cell>> rows = new List<List<Cell>>();

            //predefined singletones
            CellNumEmpty emptyCell = new CellNumEmpty();
            CellMissOperrand missOp = new CellMissOperrand();
            CellFormula formula = new CellFormula();
            CellInvval invval = new CellInvval();

            char[] operands = "+-*/".ToCharArray();

            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(fileName);
            string[] temp;
            while ((line = file.ReadLine()) != null)
            {

                temp = line.Split(' ');

                List<Cell> columns = new List<Cell>();

                for (int i = 0; i < temp.Length; i++)
                {
                    int testOut;
                    if ((temp[i] != null) && (temp[i] != "")) //if the input is valid 
                    {
                        if (temp[i] == "[]") //empty cell
                        {
                            columns.Add(emptyCell);
                        }
                        else
                            if (int.TryParse(temp[i], out testOut)) //number
                            {
                                CellNum cellNum = new CellNum(testOut);
                                columns.Add(cellNum);
                            }
                            else                            
                            {

                                if (temp[i].StartsWith("=")) //standard type of cell (cell with formula)
                                {
                                    int operandIndex = temp[i].IndexOfAny(operands);
                                    if (operandIndex == -1) //no operator 
                                    {
                                        columns.Add(missOp);
                                    }
                                    else
                                    if (operandIndex == 1 || operandIndex + 1 >= temp[i].Length)//operator at the beginning or at the end
                                    {
                                        columns.Add(formula);
                                    }
                                    else //contains correct form of formula
                                    {
                                        CellStandard standard = new CellStandard(temp[i]); 
                                        columns.Add(standard);
                                    }
                                }
                                else //invalid form of standard type
                                {
                                    columns.Add(invval);
                                }
                                
                            }
                    }
                }
                if (columns.Count > 0)
                    rows.Add(columns);
            }

            file.Close();
            return rows;
        }
    }
}

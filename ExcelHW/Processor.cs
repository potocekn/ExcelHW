using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class Processor
    {
        public List<List<Cell>> document;
        public IList<string> detectingCycleStack;
        static char[] digits = "123456789".ToCharArray();
        static char[] operands = "+-*/".ToCharArray();

        //predefined singletones
        CellInvval inval = new CellInvval();
        CellDiv0 div0 = new CellDiv0();
        CellCycle cycle = new CellCycle();
        CellError error = new CellError();
        CellMissOperrand missOP = new CellMissOperrand();
        CellFormula formula = new CellFormula();
        CellNumEmpty empty = new CellNumEmpty();

        public Processor(List<List<Cell>> sheet)
        {
            this.document = sheet;
            detectingCycleStack = new List<string>();
        }
        /// <summary>
        ///     this method calculates values of all cells that had not been resolved during reading
        /// </summary>
        public void CalculateValues()
        {
            for (int i = 0; i < document.Count; i++)
            {
                for (int j = 0; j < document[i].Count; j++)
                {
                    if (detectingCycleStack.Count > 0)
                        detectingCycleStack.Clear();
                    if (document[i][j].isFinal)
                        continue;
                    EvaluateCell(i, j);
                }
            }
        }
        /// <summary>
        ///     this method converts string of letters into int index of column
        /// </summary>
        /// <param name="column"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool GetColumnNumber(string column, out int num)
        {
            num = 0;
            int power = column.Length - 1;
            for (int i = 0; i < column.Length; i++)
            {
                var columnValue = (int)column[i] - 64; //A is index 1
                if (columnValue < 1 || columnValue > 26)//has to be a letter
                    return false;
                num += columnValue * ((int)Math.Pow(26, power));
                power--;
            }
            return true;
        }
        /// <summary>
        ///     this method checks if the given string represents valid cell
        ///     if yes, then it will return the value of the cell
        ///     else it will return invalid type of cell
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private Cell Validate(string cell)
        {
            int digitIndex = cell.IndexOfAny(digits);
            if (digitIndex == -1)
                return inval;

            int row;
            if (!int.TryParse(cell.Substring(digitIndex), out row)) 
                return inval;

            int column;
            if (!GetColumnNumber(cell.Substring(0, digitIndex), out column))
                return inval;
           
            row--;
            column--;           

            return EvaluateCell(row, column);
        }
        /// <summary>
        ///     this method evaluates cell on given indexes in the document list
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private Cell EvaluateCell(int row, int column)
        {
            Cell cell;
            if (row >= 0 && row < document.Count && column >= 0 && column < document[row].Count)
                cell = document[row][column];
            else
                cell = null;
            if (cell == null)
                return empty;

            if (cell.isFinal)
            {
                return cell;
            }
            
            if (cell.GetTypeOfCell() == 9) //cell is standard type of cell
            {
                CellStandard cellStandard = (CellStandard)cell;

                if ((cellStandard.value == "") || !(cellStandard.value[0] == '=')) //not correct form of standard type
                {
                    cellStandard.isFinal = true;                    
                    cell = inval;
                    document[row][column] = cell;
                    return cell;
                }

                string key = row.ToString() + "_" + column.ToString();
                if (detectingCycleStack.Contains(key))
                {
                    detectingCycleStack.Add(key); //adding for the second time -> cycle
                    
                    cellStandard.isFinal = true;
                    cell = cycle;
                    string[] indexes;

                    //propagate the cycle for all cells in stack
                    for (int i = detectingCycleStack.Count - 1; i > 0; i--)
                    {
                        indexes = detectingCycleStack.ElementAt(i).Split('_');
                        document[Int32.Parse(indexes[0])][Int32.Parse(indexes[1])] = cycle;
                        detectingCycleStack.RemoveAt(i);                      
                    }
                    return cell;
                }
                else
                {                   
                    detectingCycleStack.Add(key);  //add to stack for the first time                 
                }

                int operandIndex = cellStandard.value.IndexOfAny(operands);
                Cell leftValue = Validate(cellStandard.value.Substring(1, operandIndex - 1));
                Cell rightValue = Validate(cellStandard.value.Substring(operandIndex + 1));
                cell = document[row][column]; //loading the value again because it might have changed 

                if (cell.isFinal) //cell has changed during resolving other cells
                    return cell;

                if (leftValue.GetTypeOfCell() == 1 || rightValue.GetTypeOfCell() == 1) //both operands are cycle type
                {
                    int count = detectingCycleStack.Where(x => x.Equals(key)).Count(); //detecting how many times is the cell with given key in the stack 
                    
                    if (count > 1) // at least 2 times -> cycle
                    {                        
                        cellStandard.isFinal = true;
                        cell = cycle;
                        document[row][column] = cell;
                        return cell;
                    }
                    else //pointing on cycle
                    {
                        cellStandard.isFinal = true;
                        cell = error;
                        document[row][column] = cell;
                        return cell;
                    }

                }

                char operation = cellStandard.value[operandIndex];

                if (leftValue.GetTypeOfCell() == 8) //is empty cell
                    leftValue = empty;
                if (rightValue.GetTypeOfCell() == 8) //is empty cell
                    rightValue = empty;

                //left operand is number or empty cell (value 0), right operand is number type with value 0 or empty cell and operation is division 
                if (((leftValue.GetTypeOfCell() >= 7)) && ((rightValue.GetTypeOfCell() == 8)||((rightValue.GetTypeOfCell()==7)&&((CellNum)rightValue).value==0)) && operation == '/') 
                {
                    cellStandard.isFinal = true;
                    cell = div0;
                    document[row][column] = cell;
                    return cell;
                }

                if (leftValue.GetTypeOfCell() == 5 || rightValue.GetTypeOfCell() == 5) //at least one of the operands is invalid
                {
                    cellStandard.isFinal = true;
                    cell = formula;
                    document[row][column] = cell;
                    return cell;
                }

               //if both are niether number nor empty cell -> error (we cannot take their value)
                if ( (leftValue.GetTypeOfCell() < 7) || (rightValue.GetTypeOfCell() < 7) || (leftValue.GetTypeOfCell() == 9) || (rightValue.GetTypeOfCell() == 9))                     
                {
                    cellStandard.isFinal = true;
                    cell = error;
                    document[row][column] = cell;
                    return cell;
                }
                
                CellNumeric numericLeft = (CellNumeric)leftValue;
                CellNumeric numericRight = (CellNumeric)rightValue;
                int left = 0;
                if (leftValue.GetTypeOfCell() == 7) //is number
                {
                    left = numericLeft.value;                    
                }

                int right = 0;
                if (rightValue.GetTypeOfCell() == 7)//is number
                {
                    right = numericRight.value;
                }
                switch (operation)
                {
                    case '+':
                        cell = new CellNum(left + right);
                        break;
                    case '-':
                        cell = new CellNum(left - right);
                        break;
                    case '*':
                        cell = new CellNum(left * right);
                        break;
                    case '/':
                        cell = new CellNum(left / right);
                        break;
                }

                cellStandard.isFinal = true;
                document[row][column] = cell;
                return cell;
            }

            throw new Exception("Error during processing");
        }
    }
}

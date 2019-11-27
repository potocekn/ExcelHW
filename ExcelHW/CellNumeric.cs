using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    public abstract class CellNumeric : Cell
    {
        public int value;

        public abstract override byte GetTypeOfCell();


        public abstract override string ToString();
        
    }
}

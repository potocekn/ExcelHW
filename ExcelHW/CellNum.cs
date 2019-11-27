using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class CellNum : CellNumeric
    {
        
        public CellNum(int value)
        {
            this.value = value;
            this.isFinal = true;           
        }

        public override byte GetTypeOfCell()
        {
            return 7;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}

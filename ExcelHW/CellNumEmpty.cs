using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class CellNumEmpty : CellNumeric
    {
        
        public CellNumEmpty()
        {
            this.value = 0;
            this.isFinal = true;            
        }

        public override byte GetTypeOfCell()
        {
            return 8;
        }

        public override string ToString()
        {
            return "[]";
        }
    }
}

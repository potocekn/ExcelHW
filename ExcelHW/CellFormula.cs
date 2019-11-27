using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class CellFormula : Cell
    {
        public CellFormula()
        {
            this.isFinal = true;            
        }

        public override byte GetTypeOfCell()
        {
            return 4;
        }

        public override string ToString()
        {
            return "#FORMULA";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class CellDiv0 : Cell
    {
        public CellDiv0()
        {
            this.isFinal = true;            
        }

        public override byte GetTypeOfCell()
        {
            return 2;
        }

        public override string ToString()
        {
            return "#DIV0";
        }
    }
}

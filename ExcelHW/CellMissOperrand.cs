using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class CellMissOperrand : Cell
    {
        public CellMissOperrand()
        {
            this.isFinal = true;            
        }

        public override byte GetTypeOfCell()
        {
            return 6;
        }

        public override string ToString()
        {
            return "#MISSOP";
        }
    }
}

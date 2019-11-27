using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class CellCycle : Cell
    {
        public CellCycle()
        {
            this.isFinal = true;           
        }

        public override byte GetTypeOfCell()
        {
            return 1;
        }

        public override string ToString()
        {
            return "#CYCLE";
        }
    }
}

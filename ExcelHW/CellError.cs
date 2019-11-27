using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class CellError : Cell
    {
        public CellError()
        {
            this.isFinal = true;           
        }

        public override byte GetTypeOfCell()
        {
            return 3;
        }

        public override string ToString()
        {
            return "#ERROR";
        }
    }
}

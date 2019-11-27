using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class CellInvval : Cell
    {
        public CellInvval()
        {
            this.isFinal = true;            
        }

        public override byte GetTypeOfCell()
        {
            return 5;
        }

        public override string ToString()
        {
            return "#INVVAL";
        }
    }
}

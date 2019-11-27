using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    class CellStandard : Cell
    {
        public string value;

        public CellStandard(string value)
        {
            this.value = value;            
        }

        public override byte GetTypeOfCell()
        {
            return 9;
        }

        public override string ToString()
        {
            return value;
        }
    }
}

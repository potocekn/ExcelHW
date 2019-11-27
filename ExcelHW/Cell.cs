using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelHW
{
    public abstract class Cell
    {
        public bool isFinal;
        
        public abstract override string ToString();
        public abstract byte GetTypeOfCell();
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Cell
    {
        public int I { get; set; }
        public int J { get; set; } 

        public Cell()
        {

        }

        public Cell(int i, int j)
        {
            I = i;
            J = j;
        }
    }
}

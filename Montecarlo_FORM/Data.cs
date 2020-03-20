using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Montecarlo_FORM
{
    class Data : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            if( x == 0 || y == 0)
            {
                return 0;
            }

            return x.CompareTo(y);
        }
    }
}

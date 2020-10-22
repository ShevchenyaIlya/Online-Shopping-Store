using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public class MathBL
    {
        public int Sum(int num1, int num2)
        {
            int sum = num1 + num2;
            return sum;
        }
    }
}

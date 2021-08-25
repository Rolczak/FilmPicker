using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPicker.Math
{
    public static class MathHelper
    {
        public static int Lerp(int start, int end, float change)
        {
            return (int)(start + change * (end - start));
        }
    }
}

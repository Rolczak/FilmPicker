using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPicker.Helpers
{
    public static class MathHelper
    {
        public static int Lerp(int start, int end, float change)
        {
            Debug.WriteLine($"Lerping start: {start}, end: {end}, change: {change}");
            return (int)(start + (change * (end - start)));
        }
    }
}

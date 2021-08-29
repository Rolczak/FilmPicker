using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPicker.Helpers
{
    public static class StringHelper
    {
        public static string GetHoursAndMinutesFromTimespan(TimeSpan timeSpan)
        {
            return string.Format("{0:00}h {1:00}m", timeSpan.Hours, timeSpan.Minutes);
        }
    }
}

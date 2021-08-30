using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPicker.Helpers
{
    public static class StringHelper
    {
        public static string GetHoursAndMinutesFromTimespan(TimeSpan timeSpan)
        {
            Debug.WriteLine($"Getting hours and minutes from timespan: {timeSpan}");
            return string.Format("{0:00}h {1:00}m", timeSpan.Hours, timeSpan.Minutes);
        }
        public static string GenerateRandomId()
        {
            Debug.WriteLine($"Generating random string id");
            var guid = Guid.NewGuid();
            return Convert.ToBase64String(
                guid.ToByteArray())
                .Replace("=", "")
                .Replace("=", "")
                .Remove(5);
        }
    }
}

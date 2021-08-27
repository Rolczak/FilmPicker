using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPicker.Api.Models
{
    public class SearchDetails
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string FullTitle { get; set; }
        public string Plot { get; set; }
        public string Genres { get; set; }
        public float ImDbRating { get; set; }
        public SearchImages Images { get; set; }
    }
}

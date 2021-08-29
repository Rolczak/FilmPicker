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
        public DateTime ReleaseDate { get; set; }
        public float RuntimeMins { get; set; }
        public string Awards { get; set; }
        public string Directors { get; set; }
        public string Writers { get; set; }
        public string Stars { get; set; }
        public SearchImages Images { get; set; }
        public List<SearchActor> ActorList { get; set; }
        public List<SearchSimilarFilm> Similars { get; set; }
    }
}

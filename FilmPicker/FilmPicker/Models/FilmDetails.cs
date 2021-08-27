using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPicker.Models
{
    public class FilmDetails : ModelBase
    {
        private string _id;
        public string Id
        {
            get => _id; 
            set { _id = value; NotifyPropertyChanged(); }
        }

        private string _title;
        public string Title
        {
            get => _title; 
            set { _title = value; NotifyPropertyChanged(); }
        }

        private string _fullTitle;

        public string FullTitle
        {
            get => _fullTitle;
            set { _fullTitle = value; NotifyPropertyChanged(); }
        }

        private string _plot;

        public string Plot
        {
            get => _plot; 
            set { _plot = value; NotifyPropertyChanged(); }
        }

        private string _genres;

        public string Genres
        {
            get => _genres; 
            set { _genres = value; NotifyPropertyChanged(); }
        }
        private decimal _rating;

        public decimal Rating
        {
            get => _rating;
            set { _rating = value; NotifyPropertyChanged(); }
        }
        public ObservableCollection<FilmImage> Images { get; set; }
    }
}

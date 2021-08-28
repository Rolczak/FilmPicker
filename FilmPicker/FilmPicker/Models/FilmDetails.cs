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
        private double _rating;

        public double Rating
        {
            get => _rating;
            set { _rating = value; NotifyPropertyChanged(); }
        }

        private string _releaseData;
        public string ReleaseDate
        {
            get => _releaseData;
            set { _releaseData = value; NotifyPropertyChanged(); }
        }

        private string _runtime;
        public string Runtime
        {
            get => _runtime;
            set { _runtime = value; NotifyPropertyChanged(); }
        }

        private string _awards;
        public string Awards
        {
            get => _awards;
            set { _awards = value; NotifyPropertyChanged(); }
        }

        private string _directors;
        public string Directors
        {
            get => _directors;
            set { _directors = value; NotifyPropertyChanged(); }
        }

        private string _writers;
        public string Writers
        {
            get => _writers;
            set { _writers = value; NotifyPropertyChanged(); }
        }

        private string _starList;
        public string Stars
        {
            get => _starList;
            set { _starList = value; NotifyPropertyChanged(); }
        }
        public ObservableCollection<FilmImage> Images { get; set; }
        public ObservableCollection<FilmActor> Actors { get; set; }
    }
}

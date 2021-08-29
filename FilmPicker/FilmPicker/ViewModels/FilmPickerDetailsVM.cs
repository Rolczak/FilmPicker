using FilmPicker.Api;
using FilmPicker.Helpers;
using FilmPicker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPicker.ViewModels
{
    public class FilmPickerDetailsVM : ModelBase
    {
        private string filmId;
        private FilmDetails _film;

        public FilmDetails Film
        {
            get { return _film; }
            set { _film = value; NotifyPropertyChanged(); }
        }

        private bool _isSearching;

        public bool IsSearching
        {
            get => _isSearching;
            set
            {
                if (_isSearching != value)
                {
                    _isSearching = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public FilmPickerDetailsVM(string id)
        {
            filmId = id;
        }

        public async Task GetFilmDetails()
        {
            IsSearching = true;
            var searchDetails = await ApiHelper.LoadFilmDetails(filmId);
            if (searchDetails == null)
            {
                Debug.WriteLine("Error. Downloaded film details are null");
                return;
            }

            var filmDetails =  new FilmDetails
            {
                Id = searchDetails.Id,
                Title = searchDetails.Title,
                FullTitle = searchDetails.FullTitle,
                Genres = searchDetails.Genres,
                Rating = (double)searchDetails.ImDbRating / 2,
                Plot = searchDetails.Plot,
                Runtime = StringHelper.GetHoursAndMinutesFromTimespan(TimeSpan.FromMinutes(searchDetails.RuntimeMins)),
                ReleaseDate = searchDetails.ReleaseDate.ToShortDateString(),
                Awards = searchDetails.Awards,
                Directors = searchDetails.Directors,
                Stars = searchDetails.Stars,
                Writers = searchDetails.Writers,
                Images = new ObservableCollection<FilmImage>(searchDetails.Images?.Items?.Select(item => new FilmImage
                {
                    Title = item.Title,
                    Image = item.Image
                }).ToList()),
                Actors = new ObservableCollection<FilmActor>(searchDetails.ActorList?.Select(actor => new FilmActor
                {
                    Id = actor.Id,
                    Name = actor.Name,
                    Image = actor.Image,
                    AsCharacter = actor.AsCharacter
                }).ToList()),
                Similars = new ObservableCollection<SimilarFilm>(searchDetails.Similars?.Select(film => new SimilarFilm
                {
                    Id = film.Id,
                    Title = film.Title,
                    Image = film.Image
                }))
            };

            IsSearching = false;
            Film = filmDetails;
        }
    }

}

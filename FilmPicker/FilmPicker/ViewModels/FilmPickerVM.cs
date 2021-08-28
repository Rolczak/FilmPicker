using FilmPicker.Animations;
using FilmPicker.Api;
using FilmPicker.Api.Models;
using FilmPicker.Math;
using FilmPicker.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FilmPicker.ViewModels
{
    public class FilmPickerVM : ModelBase
    {
        #region CreateRandomListConfig
        private readonly int minItems = 30;
        private readonly int maxItems = 60;
        #endregion
        #region Commands
        public ICommand DeleteFilmCommand { get; private set; }
        public ICommand SelectRandomCommand { get; private set; }
        public ICommand GetSearchList { get; private set; }
        public ICommand AddCustomFilmToList { get; private set; }
        #endregion

        #region Fields
        public ObservableCollection<FilmModel> Films { get; set; }
        public ObservableCollection<SearchFilmModel> SearchFilmList { get; set; }

        private string _searchExpression;

        public string SearchExpression
        {
            get => _searchExpression;
            set
            {
                if (_searchExpression != value)
                {
                    _searchExpression = value;
                    NotifyPropertyChanged();
                }
            }
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

        private List<SearchResult> SearchListApi;
        #endregion

        public FilmPickerVM()
        {
            Films = new();
            SearchFilmList = new();
            IsSearching = false;

            DeleteFilmCommand = new DelegateCommand<string>(x =>
                {
                    Films.Remove(Films.FirstOrDefault(f => f.Id == x));
                });

            GetSearchList = new DelegateCommand(() => GetFilmsForSearchList());

            AddCustomFilmToList = new DelegateCommand(() => Films
                .Add(new FilmModel
                {
                    Id = GenerateRandomId(),
                    Title = "Enter title",
                    Multiplier = 1,
                }));
        }

        public List<string> GetRandomList()
        {
            var random = new Random();
            var randomFilmList = new List<FilmModel>();
            foreach (var film in Films)
            {
                for (int i = 0; i < film.Multiplier; i++)
                {
                    randomFilmList.Add(film);
                }
            }

            randomFilmList = randomFilmList.OrderBy(item => random.Next()).ToList();
            var randomTitleList = new List<string>();
            for (int i = 0; i < random.Next(minItems, maxItems); i++)
            {
                randomTitleList.Add(randomFilmList[random.Next(0, randomFilmList.Count)].Title);
            }
            return randomTitleList.OrderBy(item => random.Next()).ToList();
        }
        
        public async Task<FilmDetails> GetFilmDetails(string id)
        {
            var searchDetails = await ApiHelper.LoadFilmDetails(id);
            if (searchDetails == null)
            {
                Debug.WriteLine("Error. Downloaded film details are null");
                return null;
            }

            return new FilmDetails
            {
                Id = searchDetails.Id,
                Title = searchDetails.Title,
                FullTitle = searchDetails.FullTitle,
                Genres = searchDetails.Genres,
                Rating = (double)searchDetails.ImDbRating / 2,
                Plot = searchDetails.Plot,
                Runtime = GetHoursAndMinutesFromTimespan(TimeSpan.FromMinutes(searchDetails.RuntimeMins)),
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
                }).ToList())
            };
        }
        private string GetHoursAndMinutesFromTimespan(TimeSpan timeSpan)
        {
            return string.Format("{0:00}h {1:00}m", timeSpan.TotalHours, timeSpan.TotalMinutes);
        }
        private async void GetFilmsForSearchList()
        {
            IsSearching = true;
            var result = await ApiHelper.GetListForSearch(SearchExpression);
            if (result is null)
            {
                Debug.WriteLine("Result from api helper is null");
                //var toolTip = new TeachingTip
                //{
                //    Title = "Error",
                //    Subtitle = "Unexpected error when downloading data",
                //    CloseButtonCommand = RemoveToolTip
                //};
                //toolTip.CloseButtonCommandParameter = toolTip;
                //mainGrid.Children.Add(toolTip);
                //toolTip.IsOpen = true;

                return;
            }

            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                Debug.WriteLine(result.ErrorMessage);
                //var toolTip = new TeachingTip
                //{
                //    Title = "Error",
                //    Subtitle = result.ErrorMessage
                //};
                //toolTip.IsOpen = true;
                return;
            }

            SearchListApi = result.Results;
            SearchFilmList.Clear();

            foreach (var item in result.Results)
            {
                SearchFilmList.Add(new SearchFilmModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    ImageUrl = item.Image
                });
            }
            IsSearching = false;
        }

        private string GenerateRandomId()
        {
            var guid = Guid.NewGuid();
            return Convert.ToBase64String(
                guid.ToByteArray())
                .Replace("=", "")
                .Replace("=", "")
                .Remove(5);
        }
    }
}

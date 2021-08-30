using FilmPicker.Animations;
using FilmPicker.Api;
using FilmPicker.Api.Models;
using FilmPicker.Helpers;
using FilmPicker.Models;
using FilmPicker.Services;
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
        public ICommand AddFilmToList { get; private set; }
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
        #endregion

        public FilmPickerVM()
        {
            InitializeProperties();
            CreateCommands();
        }
        private void InitializeProperties()
        {
            Films = new();
            SearchFilmList = new();
            IsSearching = false;
        }
        private void CreateCommands()
        {
            DeleteFilmCommand = new DelegateCommand<string>(x =>
            {
                Films.Remove(Films.FirstOrDefault(f => f.Id == x));
            });

            GetSearchList = new DelegateCommand(() => GetFilmsForSearchList());

            AddCustomFilmToList = new DelegateCommand(() => Films
                .Add(new FilmModel
                {
                    Id = StringHelper.GenerateRandomId(),
                    Title = "Enter title",
                    Multiplier = 1,
                }));

            AddFilmToList = new DelegateCommand<string>(id =>
            {
                if (string.IsNullOrEmpty(id))
                {
                    Debug.WriteLine("Cannot add film to list id is null");
                    return;
                }
                var film = SearchFilmList.FirstOrDefault(f => f.Id == id);
                if (film == null)
                {
                    Debug.WriteLine("Not found film in list");
                    return;
                }
                Films.Add(new FilmModel
                {
                    Id = film.Id,
                    Title = film.Title,
                    Multiplier = 1
                });
            });
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

        private async void GetFilmsForSearchList()
        {
            IsSearching = true;
            var result = await ApiHelper.GetListForSearch(SearchExpression);
            if (result is null)
            {
                Debug.WriteLine("Result from api helper is null");
                ToastService.AddToast.Execute(new ToastModel
                {
                    Id = StringHelper.GenerateRandomId(),
                    Title = "Error",
                    Message = "Unexpected error when downloading data"
                });

                return;
            }

            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                Debug.WriteLine(result.ErrorMessage);
                ToastService.AddToast.Execute(new ToastModel
                {
                    Id = StringHelper.GenerateRandomId(),
                    Title = "Error",
                    Message = result.ErrorMessage
                });

                return;
            }

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
    }
}

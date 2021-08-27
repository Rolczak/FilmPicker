using FilmPicker.Animations;
using FilmPicker.Api;
using FilmPicker.Api.Models;
using FilmPicker.Controls;
using FilmPicker.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FilmPicker
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Config
        private readonly int minItems = 30;
        private readonly int maxItems = 60;
        private readonly int fromDurationMs = 50;
        private readonly int toDurationMs = 500;
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
                    NotifyPropertyChanged();
                    _searchExpression = value;
                }                            
            }
        }


        private int id = 0;
        private List<SearchResult> SearchListApi;
        #endregion

        #region Commands
        public ICommand DeleteFilmCommand { get; private set; }
        public ICommand SelectRandomCommand { get; private set; }
        public ICommand GetSearchList { get; private set; }
        public ICommand RemoveToolTip { get; private set; }
        public ICommand GetFilmDetails { get; private set; }

        #endregion

        public MainWindow()
        {
            Films = new();
            SearchFilmList = new();
            this.InitializeComponent();

            DeleteFilmCommand = new DelegateCommand<object>(x =>
            {
                var selectedList = ((IList)x).OfType<FilmModel>().ToList();
                selectedList.ForEach(x => Films.Remove(x));
            });
            SelectRandomCommand = new DelegateCommand(() => PickRandomFilm());
            GetSearchList = new DelegateCommand(() => GetFilmsForSearchList());
            RemoveToolTip = new DelegateCommand<UIElement>(tt => mainGrid.Children.Remove(tt));
            GetFilmDetails = new DelegateCommand<object>(f => 
            {
                Debug.WriteLine("works");
            });
        }

        private void addButton_click(object sender, RoutedEventArgs e)
        {
            Films.Add(new FilmModel { 
            Id = id,
            Title = "Title",
            Multiplier = 1
            });
            id++;
        }
        private List<string> GetRandomList()
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
        private async void PickRandomFilm()
        {
            winnerGrid.Children.Clear();

            PickingAnimation pickingAnimation = new PickingAnimation(GetRandomList(), fromDurationMs, toDurationMs);
            await pickingAnimation.Animate(winnerGrid);
        }

        private async void GetFilmsForSearchList()
        {
            searchListLoadIndicator.IsActive = true;
            var result = await ApiHelper.GetListForSearch(SearchExpression);
            if (result == null)
            {
                var toolTip = new TeachingTip
                {
                    Title = "Error",
                    Subtitle = "Unexpected error when downloading data",
                    CloseButtonCommand = RemoveToolTip
                };
                toolTip.CloseButtonCommandParameter = toolTip;
                mainGrid.Children.Add(toolTip);
                toolTip.IsOpen = true;

                return;
            }

            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                var toolTip = new TeachingTip
                {
                    Title = "Error",
                    Subtitle = result.ErrorMessage
                };
                toolTip.IsOpen = true;
                return;
            }

            SearchListApi = result.Results;
            SearchFilmList.Clear();
            searchListLoadIndicator.IsActive = false;
            foreach (var item in result.Results)
            {
                SearchFilmList.Add(new SearchFilmModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    ImageUrl = item.Image
                });
            }      
        }
        
        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private async void GetSearchItemDetails(object sender, RoutedEventArgs e)
        {
            if (sender is not Button)
            {
                Debug.WriteLine($"Sender is not a button. Get {sender.GetType().Name} instead");
                return;
            }
            var button = sender as Button;
            var item = SearchListApi.FirstOrDefault(f => f.Id == button.Tag);

            if (item == null)
            {
                Debug.WriteLine("Item for details not found");
                return;
            }

            var contentDialog = new FilmDetailsContentDialog(item)
            {
                CloseButtonText = "Close"
            };
            contentDialog.XamlRoot = Content.XamlRoot;
            await contentDialog.ShowAsync();

        }
    }
}

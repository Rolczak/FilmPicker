using FilmPicker.Animations;
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
    public sealed partial class MainWindow : Window
    {
        #region Fields
        public ObservableCollection<FilmModel> Films { get; set; }

        private int id = 0;
        private readonly int minItems = 30;
        private readonly int maxItems = 60;
        private readonly int fromDurationMs = 50;
        private readonly int toDurationMs = 500;
        #endregion
        #region Commands
        public ICommand DeleteFilmCommand { get; private set; }
        public ICommand SelectRandomCommand { get; private set; }
        #endregion

        public MainWindow()
        {
            Films = new();
            this.InitializeComponent();

            DeleteFilmCommand = new DelegateCommand<object>(x =>
            {
                var selectedList = ((IList)x).OfType<FilmModel>().ToList();
                selectedList.ForEach(x => Films.Remove(x));
            });
            SelectRandomCommand = new DelegateCommand(() => PickRandomFilm());
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
            
            var randomTitleList = GetRandomList();
            PickingAnimation pickingAnimation = new PickingAnimation(randomTitleList, fromDurationMs, toDurationMs);
            await pickingAnimation.Animate(winnerGrid);
        }
    }
}

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
    public sealed partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Fields
        public ObservableCollection<FilmModel> Films { get; set; }
        private string _winner;
        public string Winner
        {
            get => _winner;
            set
            {
                _winner = value;
                NotifyPropertyChanged();
            }
        }

        private int id = 0;
        private int minItems = 30;
        private int maxItems = 60;
        private int fromDurationMs = 50;
        private int toDurationMs = 500;
        #endregion
        #region Commands
        public ICommand DeleteFilmCommand { get; private set; }
        public ICommand SelectRandomCommand { get; private set; }
        #endregion

        public MainWindow()
        {
            Films = new();
            Winner = "Random Film Picker";
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

        private async void PickRandomFilm()
        {
            winnerGrid.Children.Clear();
            var random = new Random();
            var randomFilmList = new List<FilmModel>();
            foreach(var film in Films)
            {
                for(int i = 0; i < film.Multiplier; i++)
                {
                    randomFilmList.Add(film);
                }
            }

            randomFilmList = randomFilmList.OrderBy(item => random.Next()).ToList();
            var randomTitleList = new List<string>();
            for(int i = 0; i < random.Next(minItems, maxItems); i++)
            {
                randomTitleList.Add(randomFilmList[random.Next(0, randomFilmList.Count)].Title);   
            }
            randomTitleList = randomTitleList.OrderBy(item => random.Next()).ToList();

            var percentage = 0f;
            for(int i = 0;  i < randomTitleList.Count-1; i++)
            {
                var currentDurationMs = Lerp(fromDurationMs, toDurationMs, percentage);
                percentage = i / (randomTitleList.Count - 1f);
                await ChangeText(randomTitleList[i], currentDurationMs);
            }
            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            var easing = new CircleEase();
            easing.EasingMode = EasingMode.EaseOut;
            doubleAnimation.EasingFunction = easing;
            doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(toDurationMs));
            doubleAnimation.From = -100;
            doubleAnimation.To = 0;

            var randomTextBlock = new TextBlock();
            randomTextBlock.Text = randomTitleList.Last();
            randomTextBlock.Style = Application.Current.Resources["HeaderTextBlockStyle"] as Style;
            randomTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            randomTextBlock.RenderTransform = new TranslateTransform();
            winnerGrid.Children.Add(randomTextBlock);

            Storyboard.SetTarget(doubleAnimation, randomTextBlock.RenderTransform);
            Storyboard.SetTargetProperty(doubleAnimation, "Y");
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
           
        }

        private async Task ChangeText(string text, int waitTime)
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation movingAnimation= new DoubleAnimation();
            DoubleAnimation opacityAnimation = new DoubleAnimation();
            var easing = new CircleEase();
            easing.EasingMode = EasingMode.EaseInOut;
            movingAnimation.EasingFunction = easing;
            movingAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(waitTime));
            movingAnimation.From = -100;
            movingAnimation.To = 100;

            opacityAnimation.EasingFunction = easing;
            opacityAnimation.From = 0;
            opacityAnimation.To = 1;
            opacityAnimation.AutoReverse = true;
            opacityAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(waitTime/2));

            var randomTextBlock = new TextBlock();
            randomTextBlock.Text = text;
            randomTextBlock.Style = Application.Current.Resources["HeaderTextBlockStyle"] as Style;
            randomTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            randomTextBlock.RenderTransform = new TranslateTransform();
            winnerGrid.Children.Add(randomTextBlock);

            Storyboard.SetTarget(movingAnimation, randomTextBlock.RenderTransform);
            Storyboard.SetTargetProperty(movingAnimation, "Y");
            Storyboard.SetTarget(opacityAnimation, randomTextBlock);
            Storyboard.SetTargetProperty(opacityAnimation, "Opacity");
            storyboard.Children.Add(movingAnimation);
            storyboard.Children.Add(opacityAnimation);
            storyboard.Begin();
            Winner = text;
            await Task.Delay(waitTime);
            winnerGrid.Children.Remove(randomTextBlock);
            return;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int Lerp(int start, int end, float change)
        {
            return (int) (start + change * (end - start));
        }
    }
}

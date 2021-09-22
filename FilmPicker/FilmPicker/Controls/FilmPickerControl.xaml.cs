using FilmPicker.Animations;
using FilmPicker.Helpers;
using FilmPicker.Models;
using FilmPicker.Services;
using FilmPicker.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FilmPicker.Controls
{
    public sealed partial class FilmPickerControl : UserControl
    {
        #region AnimationConfig
        private readonly int fromDurationMs = 50;
        private readonly int toDurationMs = 500;
        #endregion

        public FilmPickerVM ViewModel { get; set; }
        public FilmPickerControl()
        {
            ViewModel = new();
            this.InitializeComponent();
        }

        private async void PickRandomFilm_click(object sender, RoutedEventArgs e)
        {
            winnerGrid.Children.Clear();

            var randomFilmList = ViewModel.GetRandomList();

            PickingAnimation pickingAnimation = new PickingAnimation(randomFilmList, fromDurationMs, toDurationMs);
            await pickingAnimation.Animate(winnerGrid);

            await ViewModel.SaveHistoryToFile(randomFilmList.Last());

            var redoButton = new Button
            {
                Content = new SymbolIcon(Symbol.Refresh),
                Margin = new Thickness(0, 0, 0, 10),
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            redoButton.Click += PickRandomFilm_click;

            winnerGrid.Children.Add(redoButton);
        }
        private async void GetSearchItemDetails(object sender, RoutedEventArgs e)
        {
            if (sender is not Button)
            {
                Debug.WriteLine($"Sender is not a button. Get {sender.GetType().Name} instead");
                ToastService.AddToast.Execute(new ToastModel
                {
                    Id = StringHelper.GenerateRandomId(),
                    Title = "Error",
                    Message = "Sender is not a button."
                });
                return;
            }

            var filmId = (string)(sender as Button).Tag;

            var contentDialog = new FilmDetailsContentDialog(filmId)
            {
                CloseButtonText = "Close"
            };
            contentDialog.XamlRoot = Content.XamlRoot;
            await contentDialog.ShowAsync();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button)
            {
                Debug.WriteLine($"Sender is not a button. Get {sender.GetType().Name} instead");
                ToastService.AddToast.Execute(new ToastModel
                {
                    Id = StringHelper.GenerateRandomId(),
                    Title = "Error",
                    Message = "Sender is not a button."
                });
                return;
            }
            var tag = (sender as Button).Tag;
            ViewModel.DeleteFilmCommand.Execute(tag);
        }

        private void AddFilmToList(object sender, RoutedEventArgs e)
        {
            if (sender is not Button)
            {
                Debug.WriteLine($"Sender is not a button. Get {sender.GetType().Name} instead");
                ToastService.AddToast.Execute(new ToastModel
                {
                    Id = StringHelper.GenerateRandomId(),
                    Title = "Error",
                    Message = "Sender is not a button."
                });
                return;
            }
            var tag = (sender as Button).Tag;
            ViewModel.AddFilmToList.Execute(tag);
        }

        private void AddFilmFromHistory(object sender, RoutedEventArgs e)
        {
            if (sender is not Button)
            {
                Debug.WriteLine($"Sender is not a button. Get {sender.GetType().Name} instead");
                ToastService.AddToast.Execute(new ToastModel
                {
                    Id = StringHelper.GenerateRandomId(),
                    Title = "Error",
                    Message = "Sender is not a button."
                });
                return;
            }
            var tag = (sender as Button).Tag;
            ViewModel.AddFilmFromHistory.Execute(tag);
        }
    }
}

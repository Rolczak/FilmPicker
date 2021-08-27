using FilmPicker.Api.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FilmPicker.Controls
{
    public sealed partial class FilmDetailsContentDialog : ContentDialog, INotifyPropertyChanged
    {
        public FilmDetailsContentDialog(SearchResult film)
        {
            FilmTitle = film.Title;
            Description = film.Description;
            CoverImageUrl = film.Image;
            this.InitializeComponent();
        }

        private string _filmTitle;
        public string FilmTitle
        {
            get =>_filmTitle;
            set 
            {
                if(value != _filmTitle)
                {
                    _filmTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string _coverImageUrl;

        public string CoverImageUrl
        {
            get => _coverImageUrl;
            set
            {
                if (value != _coverImageUrl)
                {
                    _coverImageUrl = value;
                    NotifyPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

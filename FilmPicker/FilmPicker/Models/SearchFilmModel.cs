using FilmPicker.Api;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPicker.Models
{
    public class SearchFilmModel : ModelBase
    {
        private string _title;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != Title)
                {
                    _title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private BitmapImage _filmCover;
        public BitmapImage FilmCover 
        {
            get => _filmCover;
            set
            {
                if (value != FilmCover)
                {
                    _filmCover = value;
                    NotifyPropertyChanged();
                }
            } 
        
        }
        public string ImageUrl { get; set; }

        public async Task LoadImage()
        {
            FilmCover = await ApiHelper.LoadImage(new Uri(ImageUrl));
            FilmCover.DecodePixelType = DecodePixelType.Logical;
            FilmCover.DecodePixelWidth = 200;
        }
    }
}

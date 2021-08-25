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

        private string _description;

        public string Description
        {
            get =>_description; 
            set 
            {
                _description = value; 
                NotifyPropertyChanged();
            }
        }

        public string ImageUrl { get; set; }
    }
}

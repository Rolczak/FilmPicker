using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FilmPicker.Models
{
    public class FilmModel : ModelBase
    {
        public int Id { get; set; }
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
            } }

        private int _muliplier;
        public int Multiplier {
            get
            {
                return _muliplier;
            }
            set
            {
                if (value != Multiplier)
                {
                    _muliplier = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}

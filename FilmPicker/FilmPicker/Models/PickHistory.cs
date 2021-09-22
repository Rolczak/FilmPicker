using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmPicker.Models
{
    public class PickHistory : ModelBase
    {
        private DateTime _pickDate;
        public DateTime PickDate { get => _pickDate; set { NotifyPropertyChanged(); _pickDate = value; } }
        private List<FilmModel> _filmModels;
        public List<FilmModel> Films { get => _filmModels; set { NotifyPropertyChanged(); _filmModels = value; } }
        private string _winnerTitle;
        public string WinnerTitle { get => _winnerTitle; set { NotifyPropertyChanged(); _winnerTitle = value; } }
    }
}

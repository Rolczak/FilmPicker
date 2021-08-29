using FilmPicker.Api.Models;
using FilmPicker.Models;
using FilmPicker.ViewModels;
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
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FilmPicker.Controls
{
    public sealed partial class FilmDetailsContentDialog : ContentDialog
    {
        public FilmPickerDetailsVM ViewModel { get; set; }
        public FilmDetailsContentDialog(string filmId)
        {
            ViewModel = new FilmPickerDetailsVM(filmId);
            this.InitializeComponent();
            ViewModel.GetFilmDetails();
        }
    }
}

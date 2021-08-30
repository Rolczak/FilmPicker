using FilmPicker.Models;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FilmPicker.Services
{
    public static class ToastService
    {
        public static ObservableCollection<ToastModel> ToastsNotificationList { get; set; }
        public static ICommand RemoveToast { get; private set; }
        public static ICommand AddToast { get; private set; }

        public static void Initialize()
        {
            Debug.WriteLine("Initializing toast service");
            ToastsNotificationList = new();
            RemoveToast = new DelegateCommand<string>(id =>
            {
                ToastsNotificationList.Remove(ToastsNotificationList.FirstOrDefault(t => t.Id == id));
            });
            AddToast = new DelegateCommand<ToastModel>(t =>
            {
                ToastsNotificationList.Add(t);
                Action<string> action = s => RemoveToast.Execute(s);
                Delay(3000, action, t.Id);
            });
        }
        private static void Delay<T>(int milliseconds, Action<T> action, T obj)
        {
            Debug.WriteLine($"Stating delayed task. Task will execute in {milliseconds/100}s");
            var t = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(milliseconds) };
            t.Tick += (o, e) => { t.Stop(); action.Invoke(obj); };
            t.Start();
        }
    }
}

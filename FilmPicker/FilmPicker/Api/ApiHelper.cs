using FilmPicker.Api.Models;
using FilmPicker.Helpers;
using FilmPicker.Models;
using FilmPicker.Services;
using Microsoft.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace FilmPicker.Api
{
    public static class ApiHelper
    {
        private const string BaseUrl = "https://imdb-api.com/en/API/";
        private static string ApiKey = ConfigurationManager.AppSettings["imbdApiKey"];
        private static HttpClient httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl), Timeout = TimeSpan.FromSeconds(30) };
        private static string SearchMovieUrl = $"SearchMovie/{ApiKey}/";
        private static string MovieDetailsUrl = $"Title/{ApiKey}/";

        public static async Task<SearchData> GetListForSearch(string expression)
        {
            Debug.WriteLine("Getting list for search list from API");
            try
            {
                var response = await httpClient.GetAsync(SearchMovieUrl + expression);
                response.EnsureSuccessStatusCode();

                var jsonContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SearchData>(jsonContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error when downloading data for search list. Exception message: {ex.Message}");
                ToastService.AddToast.Execute(new ToastModel
                {
                    Id = StringHelper.GenerateRandomId(),
                    Title = "Error",
                    Message = "Unexpected error when downloading data for seach list."
                });
                return null;
            }

        }

        public static async Task<SearchDetails> LoadFilmDetails(string Id)
        {
            Debug.WriteLine("Getting film details from API");
            try
            {
                if (string.IsNullOrWhiteSpace(Id))
                {
                    Debug.WriteLine("Error while downloading film details. Film id is null");
                    ToastService.AddToast.Execute(new ToastModel
                    {
                        Id = StringHelper.GenerateRandomId(),
                        Title = "Error",
                        Message = "Id cannot be null."
                    });
                    return null;
                }
                var response = await httpClient.GetAsync($"{MovieDetailsUrl}{Id}/images,");
                response.EnsureSuccessStatusCode();

                var jsonContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SearchDetails>(jsonContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error when downloading film details. Exception message: {ex.Message}");
                ToastService.AddToast.Execute(new ToastModel
                {
                    Id = StringHelper.GenerateRandomId(),
                    Title = "Error",
                    Message = "Unexpected error when downloading data for film details."
                });
                return null;
            }
        }
    }
}

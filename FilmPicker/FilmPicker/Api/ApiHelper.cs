using FilmPicker.Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilmPicker.Api
{
    public static class ApiHelper
    {
        private const string BaseUrl = "https://imdb-api.com/en/API/";
        private const string MoviesUrl = "SearchMovie";
        private static string ApiKey = ConfigurationManager.AppSettings["imbdApiKey"];
        private static HttpClient httpClient= new HttpClient { BaseAddress = new Uri(BaseUrl+ApiKey) };


        public static async Task<SearchData> GetListForSearch(string expression)
        {
            
            var response = await httpClient.GetAsync(MoviesUrl);
            if (response.IsSuccessStatusCode)
            {
                return null;
            }

            var jsonContent = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<SearchData>(jsonContent);
        }
       
    }
}

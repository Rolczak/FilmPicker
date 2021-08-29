﻿using FilmPicker.Api.Models;
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
        private static string ApiKey = ConfigurationManager.AppSettings["imbdApiKeyAlt"];
        private static HttpClient httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl), Timeout = TimeSpan.FromSeconds(30) };
        private static string SearchMovieUrl = $"SearchMovie/{ApiKey}/";
        private static string MovieDetailsUrl = $"Title/{ApiKey}/";

        public static async Task<SearchData> GetListForSearch(string expression)
        {
#if DEBUG
            var json = @"{ ""searchType"":""Movie"",""expression"":""inter"",""results"":[{ ""id"":""tt13594244"",""resultType"":""Title"",""image"":""https://imdb-api.com/images/original/nopicture.jpg"",""title"":""Inter"",""description"":""(in development)""},{ ""id"":""tt0816692"",""resultType"":""Title"",""image"":""https://imdb-api.com/images/original/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_Ratio0.7273_AL_.jpg"",""title"":""Interstellar"",""description"":""(2014)""},{ ""id"":""tt0113957"",""resultType"":""Title"",""image"":""https://imdb-api.com/images/original/MV5BZTg5YmM5NzktN2Q0OS00YzE1LThkOTktNTE1NTJkZmExNmIxXkEyXkFqcGdeQXVyNDAxNjkxNjQ@._V1_Ratio0.7273_AL_.jpg"",""title"":""The Net"",""description"":""(I) (1995) aka \""Accès: Interdit\""""},{ ""id"":""tt0110148"",""resultType"":""Title"",""image"":""https://imdb-api.com/images/original/MV5BYThmYjJhMGItNjlmOC00ZDRiLWEzNjUtZjU4MjA3MzY0MzFmXkEyXkFqcGdeQXVyNTI4MjkwNjA@._V1_Ratio0.7273_AL_.jpg"",""title"":""Interview with the Vampire: The Vampire Chronicles"",""description"":""(1994)""},{ ""id"":""tt0079501"",""resultType"":""Title"",""image"":""https://imdb-api.com/images/original/MV5BMTM4Mjg5ODEzMV5BMl5BanBnXkFtZTcwMDc3NDk0NA@@._V1_Ratio0.7273_AL_.jpg"",""title"":""Mad Max"",""description"":""(1979) aka \""Interceptor\""""},{ ""id"":""tt9208876"",""resultType"":""Title"",""image"":""https://imdb-api.com/images/original/MV5BODNiODVmYjItM2MyMC00ZWQyLTgyMGYtNzJjMmVmZTY2OTJjXkEyXkFqcGdeQXVyNzk3NDUzNTc@._V1_Ratio0.7273_AL_.jpg"",""title"":""The Falcon and the Winter Soldier"",""description"":""(2021) (TV Mini Series)""}],""errorMessage"":""""}";
            return JsonConvert.DeserializeObject<SearchData>(json);
#endif
            try
            {
                var response = await httpClient.GetAsync(SearchMovieUrl + expression);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var jsonContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SearchData>(jsonContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error when downloading data. Exception message: {ex.Message}");
                return null;
            }

        }

        public static async Task<BitmapImage> LoadImage(Uri url)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var inputStream = await response.Content.ReadAsStreamAsync();
                BitmapImage image = new();
                await image.SetSourceAsync(inputStream.AsRandomAccessStream());
                return image;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error when downloading image. Exception message: {ex.Message}");
                return null;
            }
        }

        public static async Task<SearchDetails> LoadFilmDetails(string Id)
        {
            #if DEBUG
            var json = @"{""id"":""tt0816692"",""title"":""Interstellar"",""originalTitle"":"""",""fullTitle"":""Interstellar (2014)"",""type"":""Movie"",""year"":""2014"",""image"":""https://imdb-api.com/images/original/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_Ratio0.6791_AL_.jpg"",""releaseDate"":""2014-11-05"",""runtimeMins"":""169"",""runtimeStr"":""2h 49mins"",""plot"":""Earth's future has been riddled by disasters, famines, and droughts. There is only one way to ensure mankind's survival: Interstellar travel. A newly discovered wormhole in the far reaches of our solar system allows a team of astronauts to go where no man has gone before, a planet that may have the right environment to sustain human life."",""plotLocal"":"""",""plotLocalIsRtl"":false,""awards"":""Top Rated Movies #29 | Won 1 Oscar. Another 43 wins & 148 nominations."",""directors"":""Christopher Nolan"",""directorList"":[{""id"":""nm0634240"",""name"":""Christopher Nolan""}],""writers"":""Jonathan Nolan, Christopher Nolan"",""writerList"":[{""id"":""nm0634300"",""name"":""Jonathan Nolan""},{""id"":""nm0634240"",""name"":""Christopher Nolan""}],""stars"":""Matthew McConaughey, Anne Hathaway, Jessica Chastain, Mackenzie Foy"",""starList"":[{""id"":""nm0000190"",""name"":""Matthew McConaughey""},{""id"":""nm0004266"",""name"":""Anne Hathaway""},{""id"":""nm1567113"",""name"":""Jessica Chastain""},{""id"":""nm3237775"",""name"":""Mackenzie Foy""}],""actorList"":[{""id"":""nm0000995"",""image"":""https://imdb-api.com/images/original/MV5BMTU4MjYxMDc3MF5BMl5BanBnXkFtZTYwNzU3MDIz._V1_Ratio0.7273_AL_.jpg"",""name"":""Ellen Burstyn"",""asCharacter"":""Murph (Older)""},{""id"":""nm0000190"",""image"":""https://imdb-api.com/images/original/MV5BMTg0MDc3ODUwOV5BMl5BanBnXkFtZTcwMTk2NjY4Nw@@._V1_Ratio0.7273_AL_.jpg"",""name"":""Matthew McConaughey"",""asCharacter"":""Cooper""},{""id"":""nm3237775"",""image"":""https://imdb-api.com/images/original/MV5BYTIyMzExODgtNzllNy00OWQwLTlhM2QtMWU1ZTI2MjgwMTQxXkEyXkFqcGdeQXVyMjQwMDg0Ng@@._V1_Ratio0.7273_AL_.jpg"",""name"":""Mackenzie Foy"",""asCharacter"":""Murph (10 Yrs.)""},{""id"":""nm0001475"",""image"":""https://imdb-api.com/images/original/MV5BMTQzMzUyNDkzNF5BMl5BanBnXkFtZTcwNTMwNTU5MQ@@._V1_Ratio0.7727_AL_.jpg"",""name"":""John Lithgow"",""asCharacter"":""Donald""},{""id"":""nm3154303"",""image"":""https://imdb-api.com/images/original/MV5BOWU1Nzg0M2ItYjEzMi00ODliLThkODAtNGEyYzRkZTBmMmEzXkEyXkFqcGdeQXVyNDk2Mzk2NDg@._V1_Ratio0.7273_AL_.jpg"",""name"":""Timothée Chalamet"",""asCharacter"":""Tom (15 Yrs.)""},{""id"":""nm0654648"",""image"":""https://imdb-api.com/images/original/MV5BOTMyODkxMzktNWMwMy00MjRlLTlmNjUtM2I0NTczZDU3YjE0XkEyXkFqcGdeQXVyOTkyMDQ2Mw@@._V1_Ratio0.7727_AL_.jpg"",""name"":""David Oyelowo"",""asCharacter"":""School Principal""},{""id"":""nm2180792"",""image"":""https://imdb-api.com/images/original/MV5BMjIwMDc5Mzk5MV5BMl5BanBnXkFtZTcwMjMzMTUxMw@@._V1_Ratio0.7273_AL_.jpg"",""name"":""Collette Wolfe"",""asCharacter"":""Ms. Hanley""},{""id"":""nm0565133"",""image"":""https://imdb-api.com/images/original/MV5BMzE5OTg2MzA4Nl5BMl5BanBnXkFtZTcwMTc3NDM1Nw@@._V1_Ratio1.5000_AL_.jpg"",""name"":""Francis X. McCarthy"",""asCharacter"":""Boots (as Francis Xavier McCarthy)""},{""id"":""nm0410347"",""image"":""https://imdb-api.com/images/original/MV5BMTU3NjI4MzM4NV5BMl5BanBnXkFtZTYwNzk4NTc4._V1_Ratio0.7273_AL_.jpg"",""name"":""Bill Irwin"",""asCharacter"":""TARS (voice)""},{""id"":""nm0004266"",""image"":""https://imdb-api.com/images/original/MV5BMTRhNzQ3NGMtZmQ1Mi00ZTViLTk3OTgtOTk0YzE2YTgwMmFjXkEyXkFqcGdeQXVyNzg5MzIyOA@@._V1_Ratio0.7727_AL_.jpg"",""name"":""Anne Hathaway"",""asCharacter"":""Brand""},{""id"":""nm0095960"",""image"":""https://imdb-api.com/images/original/MV5BMTEyODY4NjExMjVeQTJeQWpwZ15BbWU3MDQ5MjA3Njk@._V1_Ratio0.7273_AL_.jpg"",""name"":""Andrew Borba"",""asCharacter"":""Smith""},{""id"":""nm0004747"",""image"":""https://imdb-api.com/images/original/MV5BOTgyOTY5OTA5OF5BMl5BanBnXkFtZTcwNzM1MjM1Nw@@._V1_Ratio0.7273_AL_.jpg"",""name"":""Wes Bentley"",""asCharacter"":""Doyle""},{""id"":""nm0001137"",""image"":""https://imdb-api.com/images/original/MV5BMTkwOTE2NDIyNV5BMl5BanBnXkFtZTgwOTE1MTQ2NjE@._V1_Ratio0.7273_AL_.jpg"",""name"":""William Devane"",""asCharacter"":""Williams""},{""id"":""nm0000323"",""image"":""https://imdb-api.com/images/original/MV5BMjAwNzIwNTQ4Ml5BMl5BanBnXkFtZTYwMzE1MTUz._V1_Ratio0.7273_AL_.jpg"",""name"":""Michael Caine"",""asCharacter"":""Professor Brand""},{""id"":""nm1408543"",""image"":""https://imdb-api.com/images/original/MV5BOWIyNjg5YzUtZmVlOC00YTYyLWIxMjgtMGRlY2U3MDljZjg5XkEyXkFqcGdeQXVyMjI3NzY0NTA@._V1_Ratio0.7727_AL_.jpg"",""name"":""David Gyasi"",""asCharacter"":""Romilly""}],""fullCast"":null,""genres"":""Adventure, Drama, Sci-Fi"",""genreList"":[{""key"":""Adventure"",""value"":""Adventure""},{""key"":""Drama"",""value"":""Drama""},{""key"":""Sci-Fi"",""value"":""Sci-Fi""}],""companies"":""Paramount Pictures, Warner Bros., Legendary Entertainment"",""companyList"":[{""id"":""co0023400"",""name"":""Paramount Pictures""},{""id"":""co0002663"",""name"":""Warner Bros.""},{""id"":""co0159111"",""name"":""Legendary Entertainment""}],""countries"":""USA, UK, Canada"",""countryList"":[{""key"":""USA"",""value"":""USA""},{""key"":""UK"",""value"":""UK""},{""key"":""Canada"",""value"":""Canada""}],""languages"":""English"",""languageList"":[{""key"":""English"",""value"":""English""}],""contentRating"":""PG-13"",""imDbRating"":""8.6"",""imDbRatingVotes"":""1544926"",""metacriticRating"":""74"",""ratings"":null,""wikipedia"":null,""posters"":null,""images"":{""imDbId"":""tt0816692"",""title"":""Interstellar"",""fullTitle"":""Interstellar (2014)"",""type"":""Movie"",""year"":""2014"",""items"":[{""title"":""Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTc0NDQ4MjkyOF5BMl5BanBnXkFtZTgwNDE2NzUzOTE@._V1_Ratio1.3200_AL_.jpg""},{""title"":""Matthew McConaughey in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMjA3NTEwOTMxMV5BMl5BanBnXkFtZTgwMjMyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTQ0MjQ3NjE1MF5BMl5BanBnXkFtZTgwMzMyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Jessica Chastain in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTg4MTY3MDUyNl5BMl5BanBnXkFtZTgwMDMyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey and Mackenzie Foy in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMzQ5ODE2MzEwM15BMl5BanBnXkFtZTgwMTMyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""David Gyasi in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMzk3MzIzNzM5NV5BMl5BanBnXkFtZTgwNzIyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey and David Gyasi in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMjI2OTg1NjUxM15BMl5BanBnXkFtZTgwOTIyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey and Anne Hathaway in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTAyOTI5MTg5MDFeQTJeQWpwZ15BbWU4MDYyMjg4MTMx._V1_Ratio1.5000_AL_.jpg""},{""title"":""Jessica Chastain in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTg4Njk4MzY0Nl5BMl5BanBnXkFtZTgwMzIyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Wes Bentley in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BOTc0NDkxNTkwMF5BMl5BanBnXkFtZTgwNDIyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey, Timothée Chalamet, and Mackenzie Foy in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BNjQ2NTk3NTQ5OF5BMl5BanBnXkFtZTgwMTIyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Anne Hathaway and Wes Bentley in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTc0MjI0NzI0MV5BMl5BanBnXkFtZTgwMjIyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Topher Grace and Jessica Chastain in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BODg1Njg1ODQ2Ml5BMl5BanBnXkFtZTgwODEyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Anne Hathaway in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMzE3MTM0MTc3Ml5BMl5BanBnXkFtZTgwMDIyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Anne Hathaway in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTU3Nzk2MjYwMF5BMl5BanBnXkFtZTgwNjEyODgxMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Matthew McConaughey in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BNjYzNjE2NDk3N15BMl5BanBnXkFtZTgwNzEyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey and Mackenzie Foy in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTk3NTY3MzMwNl5BMl5BanBnXkFtZTgwNDEyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey and Anne Hathaway in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMjI5MTUzMTg4N15BMl5BanBnXkFtZTgwNTEyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Anne Hathaway in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTQzMjQyNzY3NV5BMl5BanBnXkFtZTgwODQ0Mjk3MjE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Matthew McConaughey, Matt Damon, Anne Hathaway, and David Gyasi in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTQ4OTQyMTMwOF5BMl5BanBnXkFtZTgwOTQ0Mjk3MjE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Casey Affleck and Jessica Chastain in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTg1NDYzMjcxOV5BMl5BanBnXkFtZTgwMDU0Mjk3MjE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey, Anne Hathaway, and David Gyasi in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMzAzMDczMTM4MV5BMl5BanBnXkFtZTgwMTU0Mjk3MjE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey, Timothée Chalamet, and Mackenzie Foy in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTc4NDkyODE3M15BMl5BanBnXkFtZTgwMTAwNDczMjE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Christopher Nolan in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTc0ODg0MjA5MF5BMl5BanBnXkFtZTgwMzMxMDc5NjE@._V1_Ratio1.7800_AL_.jpg""},{""title"":""Christopher Nolan in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMjExODc1MzcxMV5BMl5BanBnXkFtZTgwMjgyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey, Christopher Nolan, Timothée Chalamet, and Mackenzie Foy in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMjM0NzMzODEzNl5BMl5BanBnXkFtZTgwMTgyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Christopher Nolan in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTQ4NTcyNzc1MF5BMl5BanBnXkFtZTgwODcyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey and Christopher Nolan in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMjI5MzU3MzEwNl5BMl5BanBnXkFtZTgwMDgyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Christopher Nolan and Jessica Chastain in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTQ4NjgyNTI1MF5BMl5BanBnXkFtZTgwNzcyODgxMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Christopher Nolan in Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTUzMzAwNjcxOV5BMl5BanBnXkFtZTgwMjU0Mjk3MjE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BZDdjZDBmNzYtNWVkNy00M2FhLWIzYTAtZDE0MGMzYmZmMWQ5XkEyXkFqcGdeQXVyMTkxNjUyNQ@@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Matthew McConaughey and Camila Alves McConaughey at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BNzMxNjFhY2YtMmVjZS00NzdmLThhMTItMGFkZjVkMTZjNTE4XkEyXkFqcGdeQXVyMTkxNjUyNQ@@._V1_Ratio1.3200_AL_.jpg""},{""title"":""Mackenzie Foy at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTcxMDc5NzgxMF5BMl5BanBnXkFtZTgwNzAxOTQ0MzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Matthew McConaughey, Anne Hathaway, Jessica Chastain, and Mackenzie Foy at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTY0NDA5NTE4M15BMl5BanBnXkFtZTgwNTAxOTQ0MzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Anne Hathaway at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTUxMDc4MDQ4OF5BMl5BanBnXkFtZTgwMjY1MDgzMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Jessica Chastain at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMjE4MzIyNjgzOV5BMl5BanBnXkFtZTgwOTU1MDgzMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Matthew McConaughey at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMjAzOTQxNTEyM15BMl5BanBnXkFtZTgwMDY1MDgzMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Jessica Chastain at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMjMwNTAzOTAwOF5BMl5BanBnXkFtZTgwMTY1MDgzMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Matthew McConaughey, Christopher Nolan, and Emma Thomas at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BNDExMzYwODYzOF5BMl5BanBnXkFtZTgwODU1MDgzMzE@._V1_Ratio1.5000_AL_.jpg""},{""title"":""Timothée Chalamet at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMjIyMDA5ODg1Ml5BMl5BanBnXkFtZTgwODgyMzkxMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Lynda Obst at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BODYxNDQwODgwOF5BMl5BanBnXkFtZTgwNjgyMzkxMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Hans Zimmer at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTQ5NjI0NjAzM15BMl5BanBnXkFtZTgwNzgyMzkxMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Matthew McConaughey and Camila Alves McConaughey at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMjAyNzYzNTE4OV5BMl5BanBnXkFtZTgwNDgyMzkxMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Jonathan Nolan at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMTgwODkwNzEwMV5BMl5BanBnXkFtZTgwNTgyMzkxMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Christopher Nolan at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BNTg5NTk2NzkxMF5BMl5BanBnXkFtZTgwMTgyMzkxMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Kip Thorne at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BODA5ODY3NjY5OV5BMl5BanBnXkFtZTgwOTcyMzkxMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Bill Irwin at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BODM4NzkyNjcxOV5BMl5BanBnXkFtZTgwMDgyMzkxMzE@._V1_Ratio1.0000_AL_.jpg""},{""title"":""Jessica Chastain at an event for Interstellar (2014)"",""image"":""https://imdb-api.com/images/original/MV5BMjE2NDg1NjMyOF5BMl5BanBnXkFtZTgwODcyMzkxMzE@._V1_Ratio1.0000_AL_.jpg""}],""errorMessage"":""""},""trailer"":null,""boxOffice"":{""budget"":""$165,000,000 (estimated)"",""openingWeekendUSA"":""$47,510,360, 9 November 2014"",""grossUSA"":""$188,020,017"",""cumulativeWorldwideGross"":""$701,729,127""},""tagline"":""Go further."",""keywords"":""astronaut,saving the world,relativity,gravity,nasa"",""keywordList"":[""astronaut"",""saving the world"",""relativity"",""gravity"",""nasa""],""similars"":[{""id"":""tt1375666"",""title"":""Inception"",""fullTitle"":""Inception (2010)"",""year"":""2010"",""image"":""https://imdb-api.com/images/original/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_Ratio0.6737_AL_.jpg"",""plot"":""A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O."",""directors"":""Christopher Nolan"",""stars"":""Leonardo DiCaprio, Joseph Gordon-Levitt, Elliot Page"",""genres"":""Action, Adventure, Sci-Fi"",""imDbRating"":""8.8""},{""id"":""tt0468569"",""title"":""The Dark Knight"",""fullTitle"":""The Dark Knight (2008)"",""year"":""2008"",""image"":""https://imdb-api.com/images/original/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_Ratio0.6737_AL_.jpg"",""plot"":""When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice."",""directors"":""Christopher Nolan"",""stars"":""Christian Bale, Heath Ledger, Aaron Eckhart"",""genres"":""Action, Crime, Drama"",""imDbRating"":""9""},{""id"":""tt0109830"",""title"":""Forrest Gump"",""fullTitle"":""Forrest Gump (1994)"",""year"":""1994"",""image"":""https://imdb-api.com/images/original/MV5BNWIwODRlZTUtY2U3ZS00Yzg1LWJhNzYtMmZiYmEyNmU1NjMzXkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_Ratio0.6842_AL_.jpg"",""plot"":""The presidencies of Kennedy and Johnson, the Vietnam War, the Watergate scandal and other historical events unfold from the perspective of an Alabama man with an IQ of 75, whose only desire is to be reunited with his childhood sweetheart."",""directors"":""Robert Zemeckis"",""stars"":""Tom Hanks, Robin Wright, Gary Sinise"",""genres"":""Drama, Romance"",""imDbRating"":""8.8""},{""id"":""tt0137523"",""title"":""Fight Club"",""fullTitle"":""Fight Club (1999)"",""year"":""1999"",""image"":""https://imdb-api.com/images/original/MV5BMmEzNTkxYjQtZTc0MC00YTVjLTg5ZTEtZWMwOWVlYzY0NWIwXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_Ratio0.6737_AL_.jpg"",""plot"":""A nameless first person narrator (Edward Norton) attends support groups in attempt to subdue his emotional state and relieve his insomniac state. When he meets Marla (Helena Bonham Carter),... See full summary »"",""directors"":""David Fincher"",""stars"":""Brad Pitt, Edward Norton, Meat Loaf"",""genres"":""Drama"",""imDbRating"":""8.8""},{""id"":""tt7286456"",""title"":""Joker"",""fullTitle"":""Joker (2019)"",""year"":""2019"",""image"":""https://imdb-api.com/images/original/MV5BNGVjNWI4ZGUtNzE0MS00YTJmLWE0ZDctN2ZiYTk2YmI3NTYyXkEyXkFqcGdeQXVyMTkxNjUyNQ@@._V1_Ratio0.6737_AL_.jpg"",""plot"":""In Gotham City, mentally troubled comedian Arthur Fleck is disregarded and mistreated by society. He then embarks on a downward spiral of revolution and bloody crime. This path brings him face-to-face with his alter-ego: the Joker."",""directors"":""Todd Phillips"",""stars"":""Joaquin Phoenix, Robert De Niro, Zazie Beetz"",""genres"":""Crime, Drama, Thriller"",""imDbRating"":""8.4""},{""id"":""tt0111161"",""title"":""The Shawshank Redemption"",""fullTitle"":""The Shawshank Redemption (1994)"",""year"":""1994"",""image"":""https://imdb-api.com/images/original/MV5BMDFkYTc0MGEtZmNhMC00ZDIzLWFmNTEtODM1ZmRlYWMwMWFmXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_Ratio0.6737_AL_.jpg"",""plot"":""Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency."",""directors"":""Frank Darabont"",""stars"":""Tim Robbins, Morgan Freeman, Bob Gunton"",""genres"":""Drama"",""imDbRating"":""9.3""},{""id"":""tt0133093"",""title"":""The Matrix"",""fullTitle"":""The Matrix (1999)"",""year"":""1999"",""image"":""https://imdb-api.com/images/original/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVkLWI0MTEtMDllZjNkYzNjNTc4L2ltYWdlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_Ratio0.6737_AL_.jpg"",""plot"":""When a beautiful stranger leads computer hacker Neo to a forbidding underworld, he discovers the shocking truth--the life he knows is the elaborate deception of an evil cyber-intelligence."",""directors"":""Directors: Lana Wachowski, Lilly Wachowski"",""stars"":""Keanu Reeves, Laurence Fishburne, Carrie-Anne Moss"",""genres"":""Action, Sci-Fi"",""imDbRating"":""8.7""},{""id"":""tt1345836"",""title"":""The Dark Knight Rises"",""fullTitle"":""The Dark Knight Rises (2012)"",""year"":""2012"",""image"":""https://imdb-api.com/images/original/MV5BMTk4ODQzNDY3Ml5BMl5BanBnXkFtZTcwODA0NTM4Nw@@._V1_Ratio0.6737_AL_.jpg"",""plot"":""Eight years after the Joker's reign of anarchy, Batman, with the help of the enigmatic Catwoman, is forced from his exile to save Gotham City from the brutal guerrilla terrorist Bane."",""directors"":""Christopher Nolan"",""stars"":""Christian Bale, Tom Hardy, Anne Hathaway"",""genres"":""Action, Adventure"",""imDbRating"":""8.4""},{""id"":""tt0482571"",""title"":""The Prestige"",""fullTitle"":""The Prestige (2006)"",""year"":""2006"",""image"":""https://imdb-api.com/images/original/MV5BMjA4NDI0MTIxNF5BMl5BanBnXkFtZTYwNTM0MzY2._V1_Ratio0.6737_AL_.jpg"",""plot"":""After a tragic accident, two stage magicians engage in a battle to create the ultimate illusion while sacrificing everything they have to outwit each other."",""directors"":""Christopher Nolan"",""stars"":""Christian Bale, Hugh Jackman, Scarlett Johansson"",""genres"":""Drama, Mystery, Sci-Fi"",""imDbRating"":""8.5""},{""id"":""tt0993846"",""title"":""The Wolf of Wall Street"",""fullTitle"":""The Wolf of Wall Street (2013)"",""year"":""2013"",""image"":""https://imdb-api.com/images/original/MV5BMjIxMjgxNTk0MF5BMl5BanBnXkFtZTgwNjIyOTg2MDE@._V1_Ratio0.6737_AL_.jpg"",""plot"":""Based on the true story of Jordan Belfort, from his rise to a wealthy stock-broker living the high life to his fall involving crime, corruption and the federal government."",""directors"":""Martin Scorsese"",""stars"":""Leonardo DiCaprio, Jonah Hill, Margot Robbie"",""genres"":""Biography, Crime, Drama"",""imDbRating"":""8.2""},{""id"":""tt0114369"",""title"":""Se7en"",""fullTitle"":""Se7en (1995)"",""year"":""1995"",""image"":""https://imdb-api.com/images/original/MV5BOTUwODM5MTctZjczMi00OTk4LTg3NWUtNmVhMTAzNTNjYjcyXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_Ratio0.6737_AL_.jpg"",""plot"":""Two detectives, a rookie and a veteran, hunt a serial killer who uses the seven deadly sins as his motives."",""directors"":""David Fincher"",""stars"":""Morgan Freeman, Brad Pitt, Kevin Spacey"",""genres"":""Crime, Drama, Mystery"",""imDbRating"":""8.6""},{""id"":""tt0110912"",""title"":""Pulp Fiction"",""fullTitle"":""Pulp Fiction (1994)"",""year"":""1994"",""image"":""https://imdb-api.com/images/original/MV5BNGNhMDIzZTUtNTBlZi00MTRlLWFjM2ItYzViMjE3YzI5MjljXkEyXkFqcGdeQXVyNzkwMjQ5NzM@._V1_Ratio0.6842_AL_.jpg"",""plot"":""The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption."",""directors"":""Quentin Tarantino"",""stars"":""John Travolta, Uma Thurman, Samuel L. Jackson"",""genres"":""Crime, Drama"",""imDbRating"":""8.9""}],""tvSeriesInfo"":null,""tvEpisodeInfo"":null,""errorMessage"":""""}";
            return JsonConvert.DeserializeObject<SearchDetails>(json);
            #endif
            try
            {
                if (string.IsNullOrWhiteSpace(Id))
                {
                    Debug.WriteLine("Error while downloading film details. Film id is null");
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
                return null;
            }
        }
    }
}


using MovieFinder.DtoModels;
using MovieFinder.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;

namespace MovieFinder.Utils
{
    public class DataReader
    {
        public static List<MovieTitlesDto> ReadMovies(string file)
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(file);
                var parsedJson = JArray.Parse(json);
                var movies = parsedJson.ToObject<Movies[]>();

                var titlesAndYears = new List<MovieTitlesDto>();
                foreach(var movie in movies)
                {
                    var movieTitlesDto = new MovieTitlesDto(movie);
                    titlesAndYears.Add(movieTitlesDto);
                }

                return titlesAndYears; 
            }
        }
    }
}

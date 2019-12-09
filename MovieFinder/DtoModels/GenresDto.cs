using MovieFinder.Models;
using System;

namespace MovieFinder.DtoModels
{
    public class GenresDto
    {
        public bool Action { get; set; }
        public bool Adventure { get; set; }
        public bool Horror { get; set; }
        public bool Biography { get; set; }
        public bool Comedy { get; set; }
        public bool Crime { get; set; }
        public bool Thriller { get; set; }
        public bool Romance { get; set; }

        public GenresDto(Genres genres)
        {
            if (genres == null)
            {
                throw new ArgumentException($"{nameof(genres)} cannot be empty.");
            }

            Action = genres.Action;
            Adventure = genres.Adventure;
            Horror = genres.Horror;
            Biography = genres.Biography;
            Comedy = genres.Comedy;
            Crime = genres.Crime;
            Thriller = genres.Thriller;
            Romance = genres.Romance;

        }



    }
}

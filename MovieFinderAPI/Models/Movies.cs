﻿namespace MovieFinder.Models
{
    public partial class Movies
    {
        public int MovieId { get; set; }
        public int GenreId { get; set; }
        public int StreamingDataId { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public string Title { get; set; }
        public int? RunTime { get; set; }
        public int? RottenTomatoesRating { get; set; }
        public decimal? ImdbRating { get; set; }
        public string ImdbId { get; set; }
        public string Poster { get; set; }
        public string Plot { get; set; }
        public bool IsRec { get; set; }

        public virtual Genres Genre { get; set; }
        public virtual StreamingData StreamingData { get; set; }
    }
}

﻿namespace MovieFinder.Models
{
    public partial class Movies
    {
        public Movies()
        {

        }

        public int MovieId { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public string Title { get; set; }
        public int RunTime { get; set; }
    }
}

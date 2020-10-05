using MovieFinder.Models;

namespace MovieFinder.DtoModels
{
    public class RecommendedMoviesDto
    {
            public int MovieId { get; set; }
            public int GenreId { get; set; }
            public int StreamingDataId { get; set; }
            public int Year { get; set; }
            public string Director { get; set; }
            public string Title { get; set; }
            public int? RunTime { get; set; }
            public decimal? ImdbRating { get; set; }
            public decimal? RottenTomatoesRating { get; set; }
            public string ImdbId { get; set; }
            public bool IsRec { get; set; }
            public string Plot { get; set; }
            public string Poster { get; set; }
            public bool IsFavorite { get; set; }    

            public Genres Genre { get; set; }
            public StreamingData StreamingData { get; set; }
        }
}

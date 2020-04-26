using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Repository;

namespace MovieFinder.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class GenresController : Controller
    {
        private UnitOfWork _unitOfWork; 

        public GenresController(MovieFinderContext movieFinderContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext);
        }

        [HttpGet("{movieId}")]
        public IActionResult GetByMovieId(int movieId)
        {
            var genres = _unitOfWork.Genres.GetByMovieId(movieId);
            var genreDto = new GenresDto(genres); 
 
            return Ok(genreDto);
        }

        /*
        /// <summary>
        /// Endpoint used to create Movies from MovieTitlesDto. Used to manually add movies to staff reccomendations.
        /// </summary>
        /// <param name="movieInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAllMoviesWithTitle([FromBody] MovieTitlesDto movieInfo)
        {
            var imdbIds = await _moviesService.GetImdbIdsByTitle(movieInfo.MovieTitle, movieInfo.Year);
            if (imdbIds == null)
            {
                return BadRequest("Could not find imdbId.");
            }
            var movies = new List<Movies>();
            foreach (var imdbId in imdbIds)
            {
                var imdbInfo = await _moviesService.GetMovieInfo(imdbId);
                // If the imdbInfo is null, parse failed. Continue iteration.
                if (imdbInfo == null) {continue;}

                imdbInfo.IsRec = movieInfo.IsRec;

                var existingMovie = _unitOfWork.Movies.GetByImdbId(imdbInfo.ImdbId);

                // Don't save a dupe Movie.
                if (existingMovie != null)
                {
                    movies.Add(existingMovie);
                }
                else
                {
                    var movie = new Movies(imdbInfo, imdbId);
                    _unitOfWork.Movies.Add(movie);
                    _unitOfWork.SaveChanges();
                    var streamingDataDto = await _moviesService.GetStreamingData(movie.Title);

                    // Creates Synposis, Genres, and StreamingData table asscoiated with movie created.
                    FillAssociatedTables(imdbInfo, movie, streamingDataDto);
                    movies.Add(movie);
                }
            }
            return Ok(movies);
        }
        */
    }
}
 
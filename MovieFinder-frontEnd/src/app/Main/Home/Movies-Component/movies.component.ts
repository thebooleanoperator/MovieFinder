import { Component, OnInit } from "@angular/core";
import { MovieDto } from 'src/app/DTO/movie.dto';
import { MoviesService } from 'src/app/Services/movies.service';

@Component ({
    selector: 'movies',
    templateUrl: './movies.component.html',
    styleUrls: ['./movies.component.scss']
})
export class MoviesComponent implements OnInit  {
    constructor(private moviesService: MoviesService)
    {

    }

    //Data 
    showMovies: boolean;
    showPoster: boolean;
    movies: Array<MovieDto>;
    movieIndex: number;
  
    //Methods    
    ngOnInit () {
        this.setMovies();   
        this.showMovies = false;
        this.showPoster = true;
    }

    startShowing(): void {
        this.showMovies = true;
    }

    transformMovie() {
        this.showPoster = !this.showPoster;
    }

    setMovies() {
        return this.moviesService.getMovieRecs().subscribe((response: Array<MovieDto>) => {
            this.movies = response; 
            // Randomly go through the list of movies. 
            // ToDo: randomize on server.
            this.movieIndex = Math.floor(Math.random() * this.movies.length);
        })
    }

    getNextMovie() {
        this.movieIndex += 1; 
    }

    getPrevMovie() {
        this.movieIndex -= 1;
    }

    disableNext(): boolean {
        return this.movieIndex == this.movies.length - 1; 
    }

    disablePrev(): boolean {
        return this.movieIndex == 0; 
    }

    getGenres(genres): string {
        var genreBuilder = "";
        if (genres.action) {
            genreBuilder += "Action, ";
        }
        if (genres.adventure) {
            genreBuilder += "Adventure, ";
        }
        if (genres.horror) {
            genreBuilder += "Horror, ";
        }
        if (genres.biography) {
            genreBuilder += "Biography, ";
        }
        if (genres.comedy) {
            genreBuilder += "Comedy, ";
        }
        if (genres.crime) {
            genreBuilder += "Crime, ";
        }
        if (genres.thriller) {
            genreBuilder += "Thriller, ";
        }
        if (genres.romance) {
            genreBuilder += "Romance, ";
        }
        genreBuilder = genreBuilder.replace(/,\s*$/, "");
        return genreBuilder;
    }
}
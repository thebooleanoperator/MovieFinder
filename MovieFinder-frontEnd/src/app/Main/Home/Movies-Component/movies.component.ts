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
        this.showPoster = true;
    }

    getPrevMovie() {
        this.movieIndex -= 1;
        this.showPoster = true;
    }

    disableNext(): boolean {
        return this.movieIndex == this.movies.length - 1; 
    }

    disablePrev(): boolean {
        return this.movieIndex == 0; 
    }

    getGenres(genres): string {
        var genreBuilder = "";

        genres.foreEach
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

    getStreams(streams): string {
        var streamsBuilder = "";
        if (streams.netflix) {
            streamsBuilder += "Netflix";
        }
        if (streams.hbo) {
            streamsBuilder += "HBO";
        }
        if (streams.hulu) {
            streamsBuilder += "Hulu";
        }
        if (streams.disneyPlus) {
            streamsBuilder += "Disney Plus";
        }
        if (streams.amazonPrime) {
            streamsBuilder += "Amazon Prime";
        }
        if (streams.itunes) {
            streamsBuilder += "ITunes";
        }
        if (streams.googlePlay) {
            streamsBuilder += "Google Play";
        }
        return streamsBuilder;
    }
}
import { Component, OnInit } from "@angular/core";
import { MovieDto } from 'src/app/DTO/movie.dto';
import { MoviesService } from 'src/app/Services/movies.service';

@Component ({
    selector: 'movies',
    templateUrl: './movies.component.html',
    styleUrls: ['./movies.component.scss']
})
export class MoviesComponent implements OnInit {
    constructor(private moviesService: MoviesService)
    {

    }

    //Data 
    showMovies: boolean;
    movies: Array<MovieDto>;
    movieIndex: number;
    
    //Methods    
    ngOnInit() {
        this.showMovies = false;
        this.setMovies();       
    }

    setMovies() {
        return this.moviesService.getMovieRecs().subscribe((response: Array<MovieDto>) => {
            this.movies = response; 
            // Randomly go through the list of movies. 
            // ToDo: randomize on server.
            this.movieIndex = Math.floor(Math.random() * this.movies.length);
        })
    }

    startShowing() {
        this.showMovies = true;
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
}
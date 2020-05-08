import { Component, OnInit } from "@angular/core";
import { MovieDto } from 'src/app/Data/movie.dto';
import { ActivatedRoute } from '@angular/router';

@Component ({
    templateUrl: './recommendations.component.html',
    styleUrls: ['./recommendations.component.scss']
})
export class RecommendationsComponent implements OnInit  {
    constructor(private _route: ActivatedRoute){}

    //Data 
    selectedMovie: MovieDto;
    movies: MovieDto[];
    favoriteMovies: MovieDto[];
    movieIndex: number;
    
    //Methods    
    ngOnInit () {
        this._route.data
            .subscribe((data) => {
                this.movies = data.movies;
                this.favoriteMovies = data.favoriteMovies;
                // Randomly go through the list of movies.
                // ToDo: randomize on server.
                this.movieIndex = Math.floor(Math.random() * this.movies.length);
                this.selectedMovie = this.movies[this.movieIndex];
            });
    }

    getCurrentMovie() {
        return this.selectedMovie; 
    }

    changeMovie(index) {
        if (this.isLast(this.movieIndex) && index == 1) {
            this.movieIndex = 0; 
        }
        else if (this.isFirst(this.movieIndex) && index == -1) {
            this.movieIndex = this.movies.length - 1
        }
        else {
            this.movieIndex += index
        }
        this.selectedMovie = this.movies[this.movieIndex];
    }

    isFirst(index): boolean {
        return index == 0;
    }

    isLast(index): boolean {
        return index == this.movies.length - 1; 
    }
}
import { Component, OnInit, Input } from "@angular/core";
import { MovieDto } from 'src/app/Data/movie.dto';
import { ActivatedRoute } from '@angular/router';
import { FavortiesDto } from 'src/app/Data/favorites.dto';

@Component ({
    templateUrl: './recommendations.component.html',
    styleUrls: ['./recommendations.component.scss']
})
export class RecommendationsComponent implements OnInit  {
    constructor(private _route: ActivatedRoute){}

    //Data 
    selectedMovie: MovieDto;
    movies: MovieDto[];
    favoriteMovies: FavortiesDto[];
    isFavorite: boolean; 
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
                this.isFavorite = this.getIsFavorite(this.selectedMovie, this.favoriteMovies);
            });
    }

    getIsFavorite(movie: MovieDto, favorites: FavortiesDto[]): boolean {
        if (!favorites) {
            return false
        }
        return favorites.some((favorite) => {
            return favorite.MovieId == movie.movieId;
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
        this.isFavorite = this.getIsFavorite(this.selectedMovie, this.favoriteMovies);
    }

    isFirst(index): boolean {
        return index == 0;
    }

    isLast(index): boolean {
        return index == this.movies.length - 1; 
    }
}
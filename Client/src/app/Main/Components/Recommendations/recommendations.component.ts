import { Component, OnInit, Input } from "@angular/core";
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { ActivatedRoute } from '@angular/router';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { UserService } from 'src/app/Core/Services/user.service';

@Component ({
    templateUrl: './recommendations.component.html',
    styleUrls: ['./recommendations.component.scss']
})
export class RecommendationsComponent implements OnInit  {
    constructor(private _route: ActivatedRoute, private _userService: UserService){}

    //Data 
    selectedMovie: MovieDto;
    movies: MovieDto[];
    favorites: FavortiesDto[];
    isFavorite: boolean; 
    movieIndex: number;
    isGuest: boolean = this._userService.isGuest();
    error: any[] = [];

    //Methods    
    ngOnInit () {
        // Router subscription
        this._route.data
            .subscribe(
                (data) => {
                    var favoritesResolverError = data.resolvedFavorites.error;
                    var moviesResolverError = data.resolvedMovies.error;
                    if (!favoritesResolverError && ! moviesResolverError) {
                        this.movies = data.resolvedMovies.movies;
                        this.favorites = data.resolvedFavorites.favorites;
                        // Randomly go through the list of movies.
                        // ToDo: randomize on server.
                        this.movieIndex = Math.floor(Math.random() * this.movies.length);
                        this.selectedMovie = this.movies[this.movieIndex];
                        this.isFavorite = this.getIsFavorite(this.selectedMovie, this.favorites);
                    }
                    else {
                        this.error.push(favoritesResolverError);
                        this.error.push(moviesResolverError);
                    }
                }
            );
    }

    /**
     * Toggles error message in template if errors took place while resolving data.
     * @param error 
     */
    isError(error: any[]) {
        if (!error) {
            return false;
        }
        return error.length > 0 ? true : false;
    }

    /**
     * Event listner that gets called whenever child component updates favoriteMovies.
     * @param favorites 
     */
    onFavoriteAdded(favorites: FavortiesDto[]) {
        this.favorites = favorites;
        this.isFavorite = this.getIsFavorite(this.selectedMovie, this.favorites);
    }

    /**
     * Gets the selected movie.
     */
    getCurrentMovie() {
        return this.selectedMovie; 
    }

    /**
     * Changes the movie selected from movies array and passes to child movie selector to display.
     */
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
        this.isFavorite = this.getIsFavorite(this.selectedMovie, this.favorites);
    }

    /**
     * Check if a movie is in the favorites array.
     * @param movie 
     * @param favorites 
     */
    getIsFavorite(movie: MovieDto, favorites: FavortiesDto[]): boolean {
        if (!favorites) {
            return false
        }
        return favorites.some((favorite) => {
            return favorite.movieId == movie.movieId;
        });
    }  

    /**
     * Keeps changeMovie from going below 0 index.
     * @param index 
     */
    isFirst(index): boolean {
        return index == 0;
    }

    /**
     * Keeps changeMovie from going over movies array length.
     * @param index 
     */
    isLast(index): boolean {
        return index == this.movies.length - 1; 
    }
}
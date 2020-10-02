import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { FavoritesService } from 'src/app/Core/Services/favorites.service';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { AuthService } from 'src/app/Core/Services/auth-service';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';

@Component({
    selector: 'movie',
    templateUrl: './movie.component.html',
    styleUrls: ['./movie.component.scss']
})
export class MovieComponent {
    constructor(
        private _favoritesService: FavoritesService, 
        private _authService: AuthService, 
        private _toolBarService: ToolBarService){}

    // Inputs
    @Input() isGuest: boolean;
    @Input() movie: MovieDto; 
    @Input() favorite: FavortiesDto;
    @Input() isFavorite: boolean;
    
    posterError: boolean = false;
    alertUser: boolean = false;

    addToFavorites(movie: MovieDto): void {
        var favorite: FavortiesDto = new FavortiesDto(movie, this._authService.user);
        this._toolBarService.isLoading = true;
        this._favoritesService.saveFavorite(favorite)
            .subscribe(
                (favoriteDto: FavortiesDto) => {
                    // Emit to parent that favoriteMovies has been changed.
                    this._favoritesService.favoriteAdded(favoriteDto); 
                },
                (error) => {
                    if (error.status != 401) {
                        alert("Failed to add movie to favorites.");
                    }
                    this._toolBarService.isLoading = false;
                },
                () => this._toolBarService.isLoading = false
            )
    }

    removeFromFavorites(favorite: FavortiesDto) {
        this._toolBarService.isLoading = true;
        this._favoritesService.deleteFavorite(favorite.likedId)
            .subscribe(
                () => {
                    // Emit to parent that favoriteMovies has been changed.
                    this._favoritesService.favoriteRemoved(this.favorite); 
                },
                (error) => {
                    if (error.status != 401) {
                        alert("Could not remove from favorites.");
                    }
                },
                () => this._toolBarService.isLoading = false
            );
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

    useDefault(event) {
        event.srcElement.src = "/assets/images/default-poster.png";
        this.posterError = true;
    }

    isStreaming(movie:MovieDto): boolean {
        if (this.isBeingStreamed(movie)){
            return true;
        }
        else {
            return false;
        }
    }

    isBeingStreamed(movie:MovieDto): boolean {
        if (movie.streamingData.netflix) {
           return true;
        }
        if (movie.streamingData.hbo) {
            return true;
        }
        if (movie.streamingData.hulu) {
            return true;
        }
        if (movie.streamingData.disneyPlus) {
            return true;
        }
        if (movie.streamingData.amazonPrime) {
            return true;
        }
        if (movie.streamingData.iTunes) {
            return true;
        }
        if (movie.streamingData.googlePlay) {
            return true;
        }
        return false;
    }
}

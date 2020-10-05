import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { FavoritesService } from 'src/app/Core/Services/favorites.service';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { AuthService } from 'src/app/Core/Services/auth-service';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { FavoritesEventDto } from 'src/app/Data/Interfaces/favorites-event.dto';
import { StreamingDataDto } from 'src/app/Data/Interfaces/streamingData.dto';

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
    @Input() isFavorite: boolean;
    
    posterError: boolean = false;
    alertUser: boolean = false;

    addToFavorites(movie: MovieDto): void {
        var favoriteToAdd: FavortiesDto = 
            new FavortiesDto(movie, this._authService.user);
        this._favoritesService.saveFavorite(favoriteToAdd)
            .subscribe(
                (favoriteDto: FavortiesDto) => {
                    this.isFavorite = true; 
                    var favoriteEvent = new FavoritesEventDto(favoriteDto, 'add');
                    // Emit to parent that favoriteMovies has been changed.
                    this._favoritesService.favoritesUpdated(favoriteEvent);
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

    removeFromFavorites(movie: MovieDto) {
        this._toolBarService.isLoading = true;
        var favoriteToDelete: FavortiesDto = 
            new FavortiesDto(movie, this._authService.user);
        this._favoritesService.deleteFavorite(movie.movieId)
            .subscribe(
                () => {
                    this.isFavorite = false; 
                    var favoriteEvent = new FavoritesEventDto(favoriteToDelete, 'delete');
                    // Emit to parent that favoriteMovies has been changed.
                    this._favoritesService.favoritesUpdated(favoriteEvent); 
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

    isStreaming(streamingData: StreamingDataDto): boolean {
        if (streamingData.netflix) {
           return true;
        }
        if (streamingData.hbo) {
            return true;
        }
        if (streamingData.hulu) {
            return true;
        }
        if (streamingData.disneyPlus) {
            return true;
        }
        if (streamingData.amazonPrime) {
            return true;
        }
        if (streamingData.iTunes) {
            return true;
        }
        if (streamingData.googlePlay) {
            return true;
        }
        return false;
    }
}

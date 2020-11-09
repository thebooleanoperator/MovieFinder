import { Component, Input, Output } from '@angular/core';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { FavoritesService } from 'src/app/Core/Services/favorites.service';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { AuthService } from 'src/app/Core/Services/auth-service';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { StreamingDataDto } from 'src/app/Data/Interfaces/streamingData.dto';
import { RecommendedService } from 'src/app/Core/Services/recommended.service';

@Component({
    selector: 'movie',
    templateUrl: './movie.component.html',
    styleUrls: ['./movie.component.scss']
})
export class MovieComponent {
    constructor(
        private _favoritesService: FavoritesService, 
        private _authService: AuthService, 
        private _recommendedService: RecommendedService,
        private _toolBarService: ToolBarService){}

    // Inputs
    @Input() isGuest: boolean;
    @Input() movie: MovieDto; 
    @Input() isFavorite: boolean;
    @Input() isRec: boolean;
    
    posterError: boolean = false;
    alertUser: boolean = false;

    addToFavorites(movie: MovieDto): void {
        this._toolBarService.isLoading = true;
        var favoriteToAdd: FavortiesDto = 
            new FavortiesDto(movie, this._authService.user);
        this._favoritesService.saveFavorite(favoriteToAdd)
            .subscribe(
                (favoriteDto: FavortiesDto) => {
                    // Emit to parent that favoriteMovies has been changed.
                    favoriteDto.action = 'add';
                    this._favoritesService.favoritesUpdated(favoriteDto);
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
                    // Emit to parent that favoriteMovies has been changed.
                    favoriteToDelete.action = 'remove';
                    this._favoritesService.favoritesUpdated(favoriteToDelete); 
                },
                (error) => {
                    if (error.status != 401) {
                        alert("Could not remove from favorites.");
                    }
                },
                () => this._toolBarService.isLoading = false
            );
    }

    changeRecommended(index: number) {
        this._recommendedService.recommendedUpdated(index);
    }

    getGenres(genres): string {
        var genreBuilder = "";

        if (genres.action) {
            genreBuilder += "  Action  ";
        }
        if (genres.adventure) {
            genreBuilder += "  Adventure  ";
        }
        if (genres.horror) {
            genreBuilder += "  Horror  ";
        }
        if (genres.biography) {
            genreBuilder += "  Biography  ";
        }
        if (genres.comedy) {
            genreBuilder += "  Comedy  ";
        }
        if (genres.crime) {
            genreBuilder += "  Crime  ";
        }
        if (genres.thriller) {
            genreBuilder += "  Thriller  ";
        }
        if (genres.romance) {
            genreBuilder += "  Romance  ";
        }
        
        genreBuilder = genreBuilder.trim();
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

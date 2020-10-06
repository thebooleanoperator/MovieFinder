import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { MovieDialogDto } from 'src/app/Data/Interfaces/movieDialog.dto';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { DialogWatcherService } from 'src/app/Core/Services/dialog-watcher.service';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { FavoritesService } from 'src/app/Core/Services/favorites.service';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/Core/Services/auth-service';

@Component ({
    selector: "selected-movie-dialog",
    templateUrl: "./selected-movie.dialog.html",
    styleUrls: ["./selected-movie.dialog.scss"]
})
export class SelectedMovieDialog implements OnInit {
    constructor(
        @Inject(MAT_DIALOG_DATA) public data: MovieDialogDto,
        public dialogRef: MatDialogRef<SelectedMovieDialog>, 
        private _toolBarService: ToolBarService,
        private _favoritesService: FavoritesService,
        private _authService: AuthService) 
    {
        // Before closing modal update observable in dialogwatcher.
        /*this.dialogRef.beforeClosed().subscribe(() => {
            this._dialogWatcher.closedByClickOutside(this.favoriteMovies, this.movie, this.updateSearchHistory);
        });*/
    }

    // Data
    movie: MovieDto = this.data.movie;
    favorite: FavortiesDto = this.data.favorite;
    isGuest: boolean = this.data.isGuest; 
    isFavorite: boolean = this.data.isFavorite;
    updateSearchHistory: boolean = this.data.updateSearchHistory;
    showMovie: boolean = false;
    favoritesSubscription: Subscription;
    
    ngOnInit() {
        this._toolBarService.isLoading = false;

        // Favoties Subscription
        this.favoritesSubscription = this._favoritesService.favoritesUpdated$.subscribe(
            (favoriteDto: FavortiesDto) => {
                switch (favoriteDto.action) {
                    case 'add':
                        if (this.movie.movieId == favoriteDto.movieId) {
                            this.isFavorite = true;
                        }
                        break; 
                    case 'remove':
                        if (this.movie.movieId == favoriteDto.movieId) {
                            this.isFavorite = false;
                        }
                        break;
                    default:
                        break;
                }
            }
        )
    }

    /**
     * All subject subscriptions need to be unsubscribed from. 
     */
    ngOnDestroy() {
        try {
            this.favoritesSubscription.unsubscribe();
        }
        catch(error) {
            console.log('Error: ' + error);
        } 
    }
    





    getIsFavorite(movie: MovieDto, favoriteMovies: FavortiesDto[]): boolean {
        if (!favoriteMovies) {
            return false;
        }

        return favoriteMovies.some((favorite) => {
            return favorite.movieId == movie.movieId;
        })
    }
}
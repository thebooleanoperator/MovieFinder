import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MovieDto } from 'src/app/Data/movie.dto';
import { MovieDialogDto } from 'src/app/Data/movieDialog.dto';
import { FavortiesDto } from 'src/app/Data/favorites.dto';
import { DialogWatcherService } from 'src/app/Core/Services/dialog-watcher.service';

@Component ({
    selector: "selected-movie-dialog",
    templateUrl: "./selected-movie.dialog.html",
    styleUrls: ["./selected-movie.dialog.scss"]
})
export class SelectedMovieDialog {
    constructor(
        public dialogRef: MatDialogRef<SelectedMovieDialog>, 
        @Inject(MAT_DIALOG_DATA) public data: MovieDialogDto,
        private _dialogWatcher: DialogWatcherService) {
            // Before closing modal update observable in dialogwatcher.
            this.dialogRef.beforeClosed().subscribe(() => {
                this._dialogWatcher.closedByClickOutside(this.favoriteMovies);
            });
        }
    
    // Data
    showMovie: boolean = false;
    movie: MovieDto = this.data.movie;
    favoriteMovies: FavortiesDto[] = this.data.favoriteMovies;
    isFavorite: boolean = this.data.isFavorite;

    /**
     * Event listner that gets called whenever child component updates favoriteMovies.
     * @param favorites 
     */
    onFavoriteAdded(favorites: FavortiesDto[]) {
        this.favoriteMovies = favorites;
        this.isFavorite = this.getIsFavorite(this.movie, this.favoriteMovies);
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
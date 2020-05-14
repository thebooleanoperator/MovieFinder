import { Component, OnInit, AfterViewInit } from "@angular/core";
import { MovieDto } from 'src/app/Data/movie.dto';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { ActivatedRoute } from '@angular/router';
import { MoviesService } from 'src/app/Core/Services/movies.service';
import { MatDialog } from '@angular/material/dialog';
import { SelectedMovieDialog } from '../../Dialogs/Selected-Movie/selected-movie.dialog';
import { DialogWatcherService } from 'src/app/Core/Services/dialog-watcher.service';
import { FavortiesDto } from 'src/app/Data/favorites.dto';

@Component({
    templateUrl: './favorites.component.html',
    styleUrls: ['./favorites.component.scss']
})

export class FavoritesComponent implements OnInit, AfterViewInit {
    constructor(
        private _moviesService: MoviesService, 
        private _toolBarService: ToolBarService, 
        private _route: ActivatedRoute,
        private _dialog: MatDialog,
        private _dialogWatcher: DialogWatcherService){}
    
    favoriteMovies: MovieDto[]; 
    favorites: FavortiesDto[];
    page: number = 1;
    count: number = 20;
    nextExists: boolean;
    posterError: boolean = false;

    ngOnInit() {
        this._route.data.subscribe((data) => {
            this.favorites = data.favorites;
            this.favoriteMovies = data.favoriteMovies;
            this.nextExists = this.favoriteMovies.length < this.count ? false : true;
        });
    }

    ngAfterViewInit() {
         this.loadUntilScroll();
    }

    /**
     * Binds to error loading movie poster. Loads default-poster when error triggered.
     */
    useDefaultPoster(event): void {
        event.srcElement.src = "/assets/images/default-poster.png";
        this.posterError = true;
    }

    /**
     * Function used to get next page of movie favorites results from server.
     * Called by infite scroll component.
     */
    getNextFavorites(page:number, count:number): Promise<void> {
        if (this.nextExists) {
            this._toolBarService.isLoading = true;
            return this._moviesService.getFavorites(page, count).toPromise()
                .then((favoriteMoviesDtos) => {
                    if (favoriteMoviesDtos) {
                        this.favoriteMovies = this.favoriteMovies.concat(favoriteMoviesDtos); 
                        this.page += 1;
                        this.nextExists = favoriteMoviesDtos.length < this.count ? false : true;
                    }
                })
                .catch(() => alert("Unable to load favorites."))
                .finally(() => this._toolBarService.isLoading = false);
        }
    }

    /**
     * Used to load movies until vertical scroll bar appears. Measures the width of the screen,
     * when the inner width is not greater than client width, scroll bar deos not exist.
     */
    loadUntilScroll(): void {
        setTimeout(() => {
            if (this.shouldLoadNextPage()) {
                this.getNextFavorites(this.page, this.count)
                    .finally(() => this.loadUntilScroll());
            }
        }, 500); 
    }

    /**
     * Returns if the next page needs to be loaded to trigger scroll bar.
     */
    shouldLoadNextPage(): boolean {
        if (window.innerWidth > document.body.clientWidth) {
            return false;
        }
        if (!this.nextExists) {
            return false;
        }
        if (document.body.clientWidth <= 676) {
            return false;
        }
        return true;
    }

    /**
     * Opens the angular material dialogRef and passes the selectedMovie to the dialog.
     */
    openMovieDialog(movie: MovieDto, favorite: FavortiesDto[]) {
        this._dialog.open(SelectedMovieDialog, {
            width: '450px',
            data: {movie: movie, favoriteMovies: favorite, isFavorite: true}
        });

        this._dialogWatcher.closeEvent$.subscribe((favorites) => {
            this.favorites = favorites;
            this.setFavoriteMovies(this.favorites, this.favoriteMovies);
        });
    }

    /**
     * When the select movie dialog closes, the movie needs to be removed from favorites if a user
     * clicked to remove.
     * @param favorites 
     * @param favoriteMovies 
     */
    setFavoriteMovies(favorites: FavortiesDto[], favoriteMovies: MovieDto[]) {
        var favoriteIds = favorites.map((favorite) => favorite.movieId);
        this.favoriteMovies = favoriteMovies.filter((favMovie) => {
            if (favoriteIds.includes(favMovie.movieId)) {
                return favMovie;
            }
        });
    }
}
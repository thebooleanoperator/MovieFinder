import { Component, OnInit, AfterViewInit, OnDestroy } from "@angular/core";
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { ActivatedRoute } from '@angular/router';
import { MoviesService } from 'src/app/Core/Services/movies.service';
import { MatDialog } from '@angular/material/dialog';
import { SelectedMovieDialog } from '../../Dialogs/Selected-Movie/selected-movie.dialog';
import { DialogWatcherService } from 'src/app/Core/Services/dialog-watcher.service';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { Subscription } from 'rxjs';

@Component({
    templateUrl: './favorites.component.html',
    styleUrls: ['./favorites.component.scss']
})

export class FavoritesComponent implements OnInit {
    constructor(
        private _moviesService: MoviesService, 
        private _toolBarService: ToolBarService, 
        private _route: ActivatedRoute,
        private _dialog: MatDialog,
        private _dialogWatcher: DialogWatcherService){}
    
    favoriteMovies: MovieDto[]; 
    favorites: FavortiesDto[];
    skip: number = 30;
    count: number = 10;
    nextExists: boolean;
    posterError: boolean = false;
    routerSubscription: Subscription; 
    error: any[] = [];

    ngOnInit() {
        this.routerSubscription = this._route.data
            .subscribe(
                (data) => {
                    var favoritesResolverError = data.resolvedFavorites.error;
                    var favoriteMoviesResolverError = data.resolvedFavoriteMovies.error;
                    if (!favoritesResolverError && ! favoriteMoviesResolverError) {
                        this.favorites = data.resolvedFavorites.favorites;
                        this.favoriteMovies = data.resolvedFavoriteMovies.favoriteMovies;
                        // If the favorites returned from resolver are less than count, or favorites is null, we know next does not exist.
                        if (this.favoriteMovies) {
                            this.nextExists = this.favoriteMovies.length < this.count ? false : true;
                        }
                        else {
                            this.nextExists = false;
                        }
                    }
                    else {
                        this.error.push(favoritesResolverError);
                        this.error.push(favoriteMoviesResolverError);
                    }
                },
            );
    }

    isError(error: any[]) {
        if (!error) {
            return false;
        }
        return error.length > 0 ? true : false;
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
    getNextFavorites(skip:number, count:number): Subscription {
        if (this.nextExists) {
            this._toolBarService.isLoading = true;
            return this._moviesService.getFavorites(skip, count)
                .subscribe (
                    (favoriteMoviesDtos) => {
                        if (favoriteMoviesDtos) {
                            this.favoriteMovies = this.favoriteMovies.concat(favoriteMoviesDtos); 
                            this.skip += count;
                            this.nextExists = favoriteMoviesDtos.length < this.count ? false : true;
                        }
                    },
                    (error) => {
                        if (error.status != 401) {
                            alert("Unable to load favorites.");
                        }
                    },
                    () =>  this._toolBarService.isLoading = false
                );
        }
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

        this._dialogWatcher.$closeEvent.subscribe((favorites) => {
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
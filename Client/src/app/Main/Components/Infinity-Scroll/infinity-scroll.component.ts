import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { MatDialog } from '@angular/material/dialog';
import { SelectedMovieDialog } from '../../Dialogs/Selected-Movie/selected-movie.dialog';
import { DialogWatcherService } from 'src/app/Core/Services/dialog-watcher.service';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { Subscription } from 'rxjs';
import { InfitiyScrollDto } from 'src/app/Data/Interfaces/infinity-scroll.dto';

@Component({
    selector: 'infinity-scroll',
    templateUrl: './infinity-scroll.component.html',
    styleUrls: ['./infinity-scroll.component.scss']
})
export class InfinityScrollComponent implements OnInit {
    constructor(
        private _toolBarService: ToolBarService, 
        private _dialog: MatDialog,
        private _dialogWatcher: DialogWatcherService)
    {

    }

    // Inputs
    @Input() isGuest: boolean;
    @Input() movies: MovieDto[]; 
    @Input() favorites: FavortiesDto[];
    @Input() typeScrolled: string; 

    // Outputs
    @Output() getNextMovies: EventEmitter<InfitiyScrollDto> = new EventEmitter<InfitiyScrollDto>(); 

    // Data
    count: number = 10;
    posterError: boolean = false;
    nextExists: boolean;
    skip: number;
    dialogFavoritesSubscription: Subscription;
    error: any[] = [];

    // Methods
    ngOnInit() {
         // If the movies returned from resolver are less than count, we know next does not exist.
         if (this.movies) {
            this.nextExists = this.movies.length < this.count ? false : true;
        }
        else {
            this.nextExists = false;
        }

        this.dialogFavoritesSubscription = this._dialogWatcher.closeEventFavorites$.subscribe(
            (favorites) => {
                this.favorites = favorites;
                this.setmovies(this.favorites, this.movies);
            }
        );
    }

    /**
     * All subject subscriptions need to be unsubscribed from. 
     */
    ngOnDestroy() {
        this.dialogFavoritesSubscription.unsubscribe();
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
    getNext(movies: MovieDto[], count:number) {
        if (this.nextExists) {
             let infinityScroll: InfitiyScrollDto = {
                skip: movies.length,
                count: count,
                typeScrolled: this.typeScrolled
            };
            this.getNextMovies.emit(infinityScroll);
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
            data: {movie: movie, movies: favorite, isFavorite: true, updateSearchHistory: false}
        });
    }

    /**
     * When the select movie dialog closes, the movie needs to be removed from favorites if a user
     * clicked to remove.
     * @param favorites 
     * @param movies 
     */
    setmovies(favorites: FavortiesDto[], movies: MovieDto[]) {
        var favoriteIds = favorites.map((favorite) => favorite.movieId);
        this.movies = movies.filter((favMovie) => {
            if (favoriteIds.includes(favMovie.movieId)) {
                return favMovie;
            }
        });
    }

    isError(error: any[]) {
        if (!error) {
            return false;
        }
        return error.length > 0 ? true : false;
    }
}
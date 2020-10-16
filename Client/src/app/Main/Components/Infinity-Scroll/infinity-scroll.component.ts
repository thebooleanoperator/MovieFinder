import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { MatDialog } from '@angular/material/dialog';
import { SelectedMovieDialog } from '../../Dialogs/Selected-Movie/selected-movie.dialog';
import { forkJoin, Subscription } from 'rxjs';
import { InfitiyScrollDto } from 'src/app/Data/Interfaces/infinity-scroll.dto';
import { FavoritesService } from 'src/app/Core/Services/favorites.service';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { MoviesService } from 'src/app/Core/Services/movies.service';

@Component({
    selector: 'infinity-scroll',
    templateUrl: './infinity-scroll.component.html',
    styleUrls: ['./infinity-scroll.component.scss']
})
export class InfinityScrollComponent implements OnInit {
    constructor(
        private _dialog: MatDialog,
        private _favoritesService: FavoritesService,
        private _moviesService: MoviesService,
        private _toolBarService: ToolBarService)
    {

    }

    // Inputs
    @Input() isGuest: boolean;
    @Input() movies: MovieDto[]; 
    @Input() typeScrolled: string; 

    // Outputs
    @Output() getNextMovies: EventEmitter<InfitiyScrollDto> = new EventEmitter<InfitiyScrollDto>(); 

    // Data
    count: number = 10;
    posterError: boolean = false;
    nextExists: boolean;
    skip: number;
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

    showEmpty(movies: MovieDto[], type: string) {
        if (type == 'favorites') {
            return movies == null || movies.length == 0;
        } 
        if (type == 'history') {
            return movies == null || movies.length == 0;
        } 
    }

    /**
     * Opens the angular material dialogRef and passes the selectedMovie to the dialog.
     */
    openMovieDialog(movieId: number) {
        this._toolBarService.isLoading = true;

        var favorite = this._favoritesService.getByMovieId(movieId);
        var movie = this._moviesService.get(movieId);

        forkJoin([favorite, movie])
            .subscribe(
                (results) => {
                    var isFavorite = results[0] != null;
                    this._dialog.open(SelectedMovieDialog, {
                        width: '450px',
                        data: {
                            favorite: results[0], 
                            movie: results[1], 
                            isFavorite: isFavorite, 
                            updateSearchHistory: false
                        }
                    });
                },
                (error) => alert(error)
            )
    }

    isError(error: any[]) {
        if (!error) {
            return false;
        }
        return error.length > 0 ? true : false;
    }
}
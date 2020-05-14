import { Component, OnInit, AfterViewInit } from "@angular/core";
import { MovieDto } from 'src/app/Data/movie.dto';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { ActivatedRoute } from '@angular/router';
import { MoviesService } from 'src/app/Core/Services/movies.service';

@Component({
    templateUrl: './favorites.component.html',
    styleUrls: ['./favorites.component.scss']
})

export class FavoritesComponent implements OnInit, AfterViewInit {
    constructor(
        private _moviesService: MoviesService, 
        private _toolBarService: ToolBarService, 
        private _route: ActivatedRoute){}
    
    favoriteMovies: MovieDto[]; 
    page: number = 1;
    count: number = 20;
    nextExists: boolean;
    posterError: boolean = false;

    ngOnInit() {
        this._route.data.subscribe((data) => {
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
}
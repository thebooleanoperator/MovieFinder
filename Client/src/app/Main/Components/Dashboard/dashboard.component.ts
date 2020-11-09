import { Component, OnInit, OnDestroy } from '@angular/core';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { ActivatedRoute } from '@angular/router';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { Subscription } from 'rxjs';
import { SearchHistoryService } from 'src/app/Core/Services/search-history.service';
import { SearchHistoryDto } from 'src/app/Data/Interfaces/search-history.dto';
import { UserService } from 'src/app/Core/Services/user.service';
import { InfitiyScrollDto } from 'src/app/Data/Interfaces/infinity-scroll.dto';
import { FavoritesService } from 'src/app/Core/Services/favorites.service';
import { AuthService } from 'src/app/Core/Services/auth-service';
import { HostListener } from '@angular/core';
import { AppUtilities } from 'src/app/Core/Utilities/app-utilities';
import { MatTabChangeEvent } from '@angular/material';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit, OnDestroy {
    constructor(
        private _route: ActivatedRoute,
        private _favoritesService: FavoritesService, 
        private _searchHistoryService: SearchHistoryService,
        private _toolBarService: ToolBarService,
        private _userService: UserService,
        private _authService: AuthService,
        private _appUtils: AppUtilities)
        {

        }

    // Data
    /**
     * Holds an array of all movies returned from search results.
     */
    movies: MovieDto[];
    /**
     * 
     */
    recommendedMovies: MovieDto[];
    /**
     * Holds all of a users favorited movies.
     */
    favorites: FavortiesDto[];
    /**
     * 
     */
    searchedMovies: SearchHistoryDto[];
    /**
     * 
     */
    routerSubscription: Subscription;
    /**
     *
     */
    favoritesSubscription: Subscription; 
    /**
     * 
     */
    searchHistorySubscription: Subscription;
    /**
     * 
     */
    favoritesIsEmpty: boolean;
    /**
     * 
     */
    historyIsEmpty: boolean;
    /**
     * Holds the callback to correctly page the movie results.
     */
    pageEvent: any = 1; 
    /**
     * True if a catch block in a promise has been entered. 
     * Don't want to show multiple alerts to user for 1 request.
     */
    error: string[] = [];
    /**
     * 
     */
    isGuest: boolean = this._userService.isGuest();

    @HostListener('click', ['$event.target'])
    onClick(event: Event) {
        this._appUtils.clickEvent(event);
    }

    /**
     * Subscribe to resolver and handle error if one occurs.
     */
    ngOnInit() {
        // Router subscription
        this.routerSubscription = this._route.data.subscribe(
            (data) => {
                var favoritesResolverError = data.resolvedFavorites.error;
                var searchHistorResolverError = data.resolvedSearchHistory.error; 
                var recommendedMoviesResolverError = data.resolvedMovies.error;
                if (!favoritesResolverError) {
                    this.favorites = data.resolvedFavorites.favorites;
                    this.favoritesIsEmpty = this.favorites ? false : true;
                }
                if (!searchHistorResolverError) {
                    this.searchedMovies = data.resolvedSearchHistory.searchHistory
                        ? data.resolvedSearchHistory.searchHistory 
                        : [];
                    this.historyIsEmpty = this.searchedMovies.length > 0 ? false : true;
                }
                if (!recommendedMoviesResolverError) {
                    this.recommendedMovies = data.resolvedMovies.movies;
                }
                else {
                    this.error.push(favoritesResolverError);
                    this.error.push(searchHistorResolverError);
                    this.error.push(recommendedMoviesResolverError);
                }
            }
        )

        // Favoties Subscription
        this.favoritesSubscription = this._favoritesService.favoritesUpdated$.subscribe(
            (favoriteDto: FavortiesDto) => {
                switch (favoriteDto.action) {
                    case 'add':
                        this.addFavoritesHandler(favoriteDto);
                        break; 
                    case 'remove':
                        this.removeFavoritesHandler(favoriteDto);
                        break;
                    default:
                        break;
                }
            }
        )

        // SearchHistory subscription
        this.searchHistorySubscription = this._searchHistoryService.searchHistoryUpdated$
            .subscribe( 
                (searchedMovie: SearchHistoryDto) => {
                    // Filter out old history of movie if exsits.
                    this.searchedMovies = this.searchedMovies.filter((movie) => {
                        if (movie.movieId != searchedMovie.movieId) {
                            return movie;
                        }
                    })
                    this.searchedMovies.unshift(searchedMovie);
                    this.historyIsEmpty = false;
                },
                (error) => alert(error)
            )
    }

    /**
     * All subject subscriptions need to be unsubscribed from. 
     */
    ngOnDestroy() {
        try {
            this.routerSubscription.unsubscribe();
            this.favoritesSubscription.unsubscribe();
            this.searchHistorySubscription.unsubscribe();
        }
        catch(error) {
            console.log('Error: ' + error);
        } 
    }

    addFavoritesHandler(favoriteDto: FavortiesDto) {
        // Add favorite to favorites array.
        this.favorites 
            ? this.favorites.unshift(favoriteDto)
            : this.favorites = [favoriteDto];
        // Flip isFavorite to true in recommendedMovie.
        this.recommendedMovies.map((rec) => {
            if (rec.movieId == favoriteDto.movieId) {
                rec.isFavorite = true;
            }
        });
        this.favoritesIsEmpty = false;
    }

    removeFavoritesHandler(favoriteDto: FavortiesDto) {
        // Remove favorite from favorites array.
        this.favorites = this.favorites.filter((fav) => {
            return fav.movieId != favoriteDto.movieId;
        });
        // Flip isFavorite to false in recommendedMovie.
        this.recommendedMovies.map((rec) => {
            if (rec.movieId == favoriteDto.movieId) {
                rec.isFavorite = false;
            }
        });
        this.favoritesIsEmpty = this.favorites.length > 0 ? false : true;
    }

    /**
     * Listens for output from InfinityScrollComponent to page history or favorites.
     * @param infinityScrollDto 
     */
    onGetNextMovies(infinityScrollDto: InfitiyScrollDto): Subscription {
        switch(infinityScrollDto.typeScrolled) {
            case "favorites":
                return this.getNextFavorites(infinityScrollDto.skip, infinityScrollDto.count);
            case "history":
                return this.getNextSearchHistory(infinityScrollDto.skip, infinityScrollDto.count);
        }
    }   

    /**
     * 
     * @param skip 
     * @param count 
     */
    getNextFavorites(skip: number, count: number): Subscription {
        this._toolBarService.isLoading = true;
            return this._favoritesService.getAll(skip, count)
                .subscribe (
                    (favoriteDtos) => {
                        if (favoriteDtos) {
                            this.favorites = this.favorites.concat(favoriteDtos);
                        }
                    },
                    (error) => {
                        alert("Unable to load favorites.");
                        console.log(error);
                    },
                    () => this._toolBarService.isLoading = false
                );
    }

    getNextSearchHistory(skip: number, count: number) {
        this._toolBarService.isLoading = true;
        return this._searchHistoryService.getAll(skip, count)
            .subscribe(
                (searchedHistory: SearchHistoryDto[]) => {
                    if (searchedHistory) {
                        this.searchedMovies = this.searchedMovies.concat(searchedHistory);
                    }
                },
                (error) => {
                    alert("Unable to load favorites.");
                    console.log(error);
                },
                () => this._toolBarService.isLoading = false
            )
    }

    onAddFavorites(movie: MovieDto) {
        var favorite: FavortiesDto = new FavortiesDto(movie, this._authService.user);
        this._toolBarService.isLoading = true;
        this._favoritesService.saveFavorite(favorite)
            .subscribe(
                (data) => {
                    this.favorites ? this.favorites.push(data) : this.favorites = [data];
                },
                (error) => {
                    alert("Failed to add movie to favorites.");
                    this._toolBarService.isLoading = false;
                },
                () => this._toolBarService.isLoading = false
            )
    }

    /**
     * Used to get isFavorite value to send to dialog.
     * @param movie 
     * @param favoriteMovies 
     */
    getIsFavorite(movie: MovieDto, favoriteMovies: FavortiesDto[]): boolean {
        if (!favoriteMovies) {
            return false;
        }

        return favoriteMovies.some((favorite) => {
            return favorite.movieId == movie.movieId;
        })
    }

    /**
     * Toggles error message in template if errors took place while resolving data.
     * @param error 
     */
    isError(error: any[]) {
        if (!error) {
            return false;
        }
        return error.length > 0 ? true : false;
    }
}

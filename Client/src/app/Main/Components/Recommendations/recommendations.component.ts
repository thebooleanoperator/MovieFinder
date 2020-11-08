import { Component, OnInit, Input } from "@angular/core";
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { FavortiesDto } from 'src/app/Data/Interfaces/favorites.dto';
import { FavoritesService } from 'src/app/Core/Services/favorites.service';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { Subscription } from 'rxjs';
import { RecommendedService } from 'src/app/Core/Services/recommended.service';

@Component ({
    selector: 'recommended',
    templateUrl: './recommendations.component.html',
    styleUrls: ['./recommendations.component.scss']
})
export class RecommendationsComponent implements OnInit  {
    constructor(
        private _favoritesService: FavoritesService,
        private _recommendedService: RecommendedService,
        private _toolBarService: ToolBarService)
    {
            
    }

    // Inputs 
    @Input() recommendedMovies: MovieDto[];
    @Input() isGuest: boolean;
    
    // Data
    selectedMovie: MovieDto;
    movieIndex: number;
    recommendedSubscription: Subscription; 

    //Methods    
    ngOnInit () {
        // Randomly go through the list of recommendedMovies.
        // ToDo: randomize on server.
        this.movieIndex = Math.floor(Math.random() * this.recommendedMovies.length);
        this.selectedMovie = this.recommendedMovies[this.movieIndex];

        this.recommendedSubscription = this._recommendedService.recommendedUpdated$.subscribe(
            (index: number) => this.changeMovie(index)
        )
    }

    /**
     * Changes the movie selected from recommendedMovies array and passes to child movie selector to display.
     */
    changeMovie(index) {
        if (this.isLast(this.movieIndex) && index == 1) {
            this.movieIndex = 0; 
        }
        else if (this.isFirst(this.movieIndex) && index == -1) {
            this.movieIndex = this.recommendedMovies.length - 1
        }
        else {
            this.movieIndex += index
        }
        this.selectedMovie = this.recommendedMovies[this.movieIndex];
    }

    /**
     * Check if a movie is in the favorites array.
     * @param movie 
     * @param favorites 
     */
    getIsFavorite(movieId: number): void {
        this._toolBarService.isLoading = true;
        this._favoritesService.getByMovieId(movieId)
            .subscribe(
                (favoriteDto: FavortiesDto) => favoriteDto != null,
                (error) => alert(error),
                () => this._toolBarService.isLoading = false
            );
    }  

    /**
     * Keeps changeMovie from going below 0 index.
     * @param index 
     */
    isFirst(index): boolean {
        return index == 0;
    }

    /**
     * Keeps changeMovie from going over recommendedMovies array length.
     * @param index 
     */
    isLast(index): boolean {
        return index == this.recommendedMovies.length - 1; 
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

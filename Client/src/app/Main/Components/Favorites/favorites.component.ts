import { Component, OnInit } from "@angular/core";
import { MovieDto } from 'src/app/Data/movie.dto';
import { FavoritesService } from 'src/app/Core/Services/favorites.service';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';

@Component({
    templateUrl: './favorites.component.html',
    styleUrls: ['./favorites.component.scss']
})

export class FavoritesComponent implements OnInit {
    constructor(private _favoritesService: FavoritesService, private _toolBarService: ToolBarService){}
    
    favoriteMovies: MovieDto; 

    /**
     * Using setTimeout prevents synchronous call while rendering page.
     * Otherwise throws ExpressionHasChangedException.
     * Reference: https://blog.angular-university.io/angular-debugging/
     */
    ngOnInit() {        
        setTimeout(() => {
            this._toolBarService.isLoading = true;
            this._favoritesService.getFavoriteMovies().toPromise()
                .then((moviesDto) => this.favoriteMovies = moviesDto)
                .catch(() => alert("Unable to get liked movies."))
                .finally(() => this._toolBarService.isLoading = false);
        });
    }
}
import { Component, OnInit } from "@angular/core";
import { MovieDto } from 'src/app/Data/movie.dto';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { ActivatedRoute } from '@angular/router';
import { MoviesService } from 'src/app/Core/Services/movies.service';

@Component({
    templateUrl: './favorites.component.html',
    styleUrls: ['./favorites.component.scss']
})

export class FavoritesComponent implements OnInit {
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

    useDefaultPoster(event) {
        event.srcElement.src = "/assets/images/default-poster.png";
        this.posterError = true;
    }

    getNextFavorites(page:number, count:number) {
        if (this.nextExists) {
            this._toolBarService.isLoading = true;
            this._moviesService.getFavorites(page, count).toPromise()
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
}
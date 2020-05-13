import { Component, OnInit } from "@angular/core";
import { MovieDto } from 'src/app/Data/movie.dto';
import { FavoritesService } from 'src/app/Core/Services/favorites.service';
import { ToolBarService } from 'src/app/Core/Services/tool-bar.service';
import { ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: './favorites.component.html',
    styleUrls: ['./favorites.component.scss']
})

export class FavoritesComponent implements OnInit {
    constructor(
        private _favoritesService: FavoritesService, 
        private _toolBarService: ToolBarService, 
        private _route: ActivatedRoute){}
    
    favoriteMovies: MovieDto; 
    posterError: boolean = false;

    ngOnInit() {
        this._route.data.subscribe((data) => {
            this.favoriteMovies = data.favoriteMovies;
        })
    }

    useDefaultPoster(event) {
        event.srcElement.src = "/assets/images/default-poster.png";
        this.posterError = true;
    }
}
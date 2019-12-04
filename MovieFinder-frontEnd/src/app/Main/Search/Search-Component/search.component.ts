import { Component, OnInit } from '@angular/core';
import { MovieDto } from 'src/app/DTO/movie.dto';
import { MoviesService } from 'src/app/Services/movies.service';

@Component({
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent {
    showSearch: boolean = false; 
    movies: Array<MovieDto> = []; 

    constructor(private moviesService: MoviesService){}

    toggleSearch(): void {
        this.showSearch = !this.showSearch;
    }

    searchMovies(search:string) : void {
        this.moviesService.getMoviesByTitle(search).subscribe((response) => {
            console.log(response);
        })
    }
}
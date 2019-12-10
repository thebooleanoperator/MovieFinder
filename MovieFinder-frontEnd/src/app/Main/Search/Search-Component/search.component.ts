import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MovieDto } from 'src/app/DTO/movie.dto';
import { MoviesService } from 'src/app/Services/movies.service';
import { GenresDto } from 'src/app/Dto/genres.dto';
import { GenresService } from 'src/app/Services/genres-service';

@Component({
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent {
    showSearch: boolean = false; 
    movies: Array<MovieDto>;
    genres: GenresDto;  
    public displayedColumns : string[] = ['Title', 'Genre', 'Director', 'Year', 'RunTime'];

    constructor(private moviesService: MoviesService, private genresService: GenresService){}

    toggleSearch(): void {
        this.showSearch = !this.showSearch;
    }

    searchMovies(search:string) : void {
        this.moviesService.getMoviesByTitle(search).subscribe((response) => {
            this.movies = response;
        })
    }

    getGenres(movieId:number) : string {
        var resp = this.genresService.getGenresFromMovieId(movieId).subscribe((response) => {
           this.genres = response;
           var genresString = this.getGenresString();
           return genresString;
        })
    }

    getGenresString(): string {
        var genreString = "";
        if (this.genres.Crime) {
            genreString += "Crime, "
        }
        if (this.genres.Thriller) {
            genreString += "Thriller, "
        }

        return genreString
    }
}
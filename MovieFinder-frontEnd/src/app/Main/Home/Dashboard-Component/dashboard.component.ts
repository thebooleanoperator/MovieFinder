import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MovieDto } from 'src/app/DTO/movie.dto';
import { MoviesService } from 'src/app/Services/movies.service';
import { GenresDto } from 'src/app/Dto/genres.dto';
import { GenresService } from 'src/app/Services/genres-service';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
    showSearch: boolean = false; 
    movies: Array<MovieDto>;
    public displayedColumns : string[] = ['Title', 'Year'];

    constructor(private moviesService: MoviesService, private genresService: GenresService){}

    toggleSearch(): void {
        this.showSearch = !this.showSearch;
        if (!this.showSearch) {
            this.movies = null;
        }
    }

    searchMovies(search:string) : void {
        this.moviesService.getMoviesByTitle(search).subscribe((response) => {
            this.movies = response;
        })
    }

    formatGenres(genres): string {
        var genreString = "";
        if (genres.action) {
            genreString += "Action "
        }
        if (genres.adventure) {
            genreString += "Adventure "
        }
        if (genres.horror) {
            genreString += "Horror "
        }
        if (genres.biography) {
            genreString += "Biography "
        }
        if (genres.comedy) {
            genreString += "Comedy "
        }
        if (genres.romance) {
            genreString += "Romance "
        }
        if (genres.crime) {
            genreString += "Crime "
        }
        if (genres.thriller) {
            genreString += "Thriller "
        }

        return genreString
    }
    
    isOnNetflix(netflixId): boolean {
        return netflixId ? true : false;
    }
}
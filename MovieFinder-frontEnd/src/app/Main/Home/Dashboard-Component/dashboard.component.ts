import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MovieDto } from 'src/app/DTO/movie.dto';
import { MoviesService } from 'src/app/Services/movies.service';
import { GenresService } from 'src/app/Services/genres-service';
import { ImdbIdDto } from 'src/app/Dto/imdbId.dto';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
    movies: Array<MovieDto>;
    public displayedColumns : string[] = ['Title', 'Year'];

    constructor(private moviesService: MoviesService){}

    searchMovies(search:string) : void {
        if (search) {
            this.moviesService.getImdbIdsByTitle(search).subscribe((response) => {
                this.movies = response;
            })
        }
    }

    test(imdbId: ImdbIdDto): void {
        this.moviesService.getMovieByImdbId(imdbId.imdbId).subscribe((response) => {
            console.log(response);
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
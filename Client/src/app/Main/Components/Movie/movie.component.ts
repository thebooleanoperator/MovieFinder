import { Component, Input } from '@angular/core';
import { MovieDto } from 'src/app/Data/movie.dto';

@Component({
    selector: 'movie',
    templateUrl: './movie.component.html',
    styleUrls: ['./movie.component.scss']
})
export class MovieComponent {
    // Data
    @Input() movie: MovieDto; 
    
    posterError: boolean = false;

    getGenres(genres): string {
        var genreBuilder = "";

        genres.foreEach
        if (genres.action) {
            genreBuilder += "Action, ";
        }
        if (genres.adventure) {
            genreBuilder += "Adventure, ";
        }
        if (genres.horror) {
            genreBuilder += "Horror, ";
        }
        if (genres.biography) {
            genreBuilder += "Biography, ";
        }
        if (genres.comedy) {
            genreBuilder += "Comedy, ";
        }
        if (genres.crime) {
            genreBuilder += "Crime, ";
        }
        if (genres.thriller) {
            genreBuilder += "Thriller, ";
        }
        if (genres.romance) {
            genreBuilder += "Romance, ";
        }
        genreBuilder = genreBuilder.replace(/,\s*$/, "");
        return genreBuilder;
    }

    useDefault(event) {
        event.srcElement.src = "/assets/images/default-poster.png";
        this.posterError = true;
    }

    isStreaming(movie:MovieDto): boolean {
        if (this.isBeingStreamed(movie)){
            return true;
        }
        else {
            return false;
        }
    }

    isBeingStreamed(movie:MovieDto): boolean {
        if (movie.streamingData.netflix) {
           return true;
        }
        if (movie.streamingData.hbo) {
            return true;
        }
        if (movie.streamingData.hulu) {
            return true;
        }
        if (movie.streamingData.disneyPlus) {
            return true;
        }
        if (movie.streamingData.amazonPrime) {
            return true;
        }
        if (movie.streamingData.iTunes) {
            return true;
        }
        if (movie.streamingData.googlePlay) {
            return true;
        }
        return false;
    }
}
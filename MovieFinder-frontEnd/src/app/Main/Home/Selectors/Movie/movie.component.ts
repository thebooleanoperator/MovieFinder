import { Component, Input, OnInit } from '@angular/core';
import { MovieDto } from 'src/app/DTO/movie.dto';

@Component({
    selector: 'movie',
    templateUrl: './movie.component.html',
    styleUrls: ['./movie.component.scss']
})
export class MovieComponent implements OnInit{
    // Data
    @Input() test: string;
    @Input() movie: MovieDto; 

    showPoster: boolean = true;
    
    ngOnInit() {
        console.log(this.movie);
    }

    transformMovie() {
        this.showPoster = !this.showPoster;
    }

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

    getStreams(streams): string {
        var streamsBuilder = "";
        if (streams.netflix) {
            streamsBuilder += "Netflix";
        }
        if (streams.hbo) {
            streamsBuilder += "HBO";
        }
        if (streams.hulu) {
            streamsBuilder += "Hulu";
        }
        if (streams.disneyPlus) {
            streamsBuilder += "Disney Plus";
        }
        if (streams.amazonPrime) {
            streamsBuilder += "Amazon Prime";
        }
        if (streams.itunes) {
            streamsBuilder += "ITunes";
        }
        if (streams.googlePlay) {
            streamsBuilder += "Google Play";
        }
        return streamsBuilder;
    }

    useDefault(event) {
        event.srcElement.src = "/assets/images/default-poster.png"
    }
}
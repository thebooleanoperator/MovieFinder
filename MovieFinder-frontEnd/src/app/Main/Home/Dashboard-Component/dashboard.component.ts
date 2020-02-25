import { Component, OnInit } from '@angular/core';
import { LikedMoviesService } from 'src/app/Services/liked-movies.service'
import { MovieDto } from '../../../DTO/movie.dto'
import { UserDto } from 'src/app/Dto/user.dto';
import { SignInService } from 'src/app/Services/sign-in.service';
import { MoviesService } from 'src/app/Services/movies.service';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
    constructor(private likedMoviesService: LikedMoviesService, private signInService : SignInService){}
    //Data
    public user : UserDto = this.signInService.user;
    public likedMovies: MovieDto[]; 
    public displayedColumns : string[] = ['Title', 'Genre', 'Director', 'Year', 'ImdbRating', 'RunTime', 'OnNetflix'];

    //Methods
    ngOnInit() {
        this.likedMoviesService.getLikedMovies()
            .then((response: MovieDto[]) => this.likedMovies = response);
    }
}


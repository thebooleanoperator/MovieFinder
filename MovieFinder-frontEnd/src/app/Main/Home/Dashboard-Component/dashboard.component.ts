import { Component, OnInit } from '@angular/core';
import { LikedMoviesService } from 'src/app/Services/liked-movies.service'
import { MovieDto } from '../../../DTO/movie.dto'
import { UserDto } from 'src/app/Dto/user.dto';
import { UserService } from 'src/app/Services/user.service';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
    constructor(private likedMoviesService: LikedMoviesService, private userService : UserService){}
    //Data
    public user : UserDto;
    public likedMovies: MovieDto[]; 
    public displayedColumns : string[] = ['Title', 'Genre', 'Director', 'Year', 'ImdbRating', 'RunTime', 'OnNetflix'];

    //Methods
    ngOnInit() {
        this.likedMoviesService.getLikedMovies().subscribe((response) => {
            this.likedMovies = response; 
        });
        this.userService.getUser().subscribe((response : UserDto) => {
            this.user = response;
        })
    }
}

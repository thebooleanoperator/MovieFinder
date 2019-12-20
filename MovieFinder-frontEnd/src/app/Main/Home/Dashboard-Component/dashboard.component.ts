import { Component, OnInit } from '@angular/core';
import { LikedMoviesService } from 'src/app/Services/liked-movies.service'
import { UserService } from 'src/app/Services/user.service';
import { MovieDto } from '../../../DTO/movie.dto'
import { UserDto } from 'src/app/DTO/user.dto';

@Component({
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
    user: UserDto = JSON.parse(localStorage.getItem("userInfo"))
    public likedMovies: MovieDto[]; 
    public displayedColumns : string[] = ['Title', 'Genre', 'Director', 'Year', 'ImdbRating', 'RunTime', 'OnNetflix'];
    constructor(private likedMoviesService: LikedMoviesService, private userService: UserService){}

    
   ngOnInit() {
       this.likedMoviesService.getLikedMovies(2).subscribe((response) => {
           this.likedMovies = response; 
       });
       this.likedMovies
   }
}

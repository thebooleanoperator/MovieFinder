import { Component, OnInit } from '@angular/core';
import { LikedMoviesService } from 'src/app/Services/liked-movies.service'
import { UserService } from 'src/app/Services/user.service';
import { MovieDto } from '../../DTO/movie.dto'

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
    public likedMovies: MovieDto[]; 
    public displayedColumns : string[] = ['Title', 'Genre', 'Director', 'Year', 'RunTime'];
    constructor(private likedMoviesService: LikedMoviesService, private userService: UserService){}

    
   ngOnInit() {
       this.likedMoviesService.getLikedMovies(this.userService.userInfo.userId).subscribe((response) => {
           this.likedMovies = response; 
       })
   }
}

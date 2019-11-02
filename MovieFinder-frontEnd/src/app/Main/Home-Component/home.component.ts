import { Component, OnInit } from '@angular/core';
import { LikedMoviesService } from 'src/app/Services/liked-movies.service'
import { UserService } from 'src/app/Services/user.service';

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
    public likedMovies: Object[]; 
    constructor(private likedMoviesService: LikedMoviesService, private userService: UserService){}

    
   ngOnInit() {
       this.likedMoviesService.getLikedMovies(this.userService.userInfo.userId).subscribe((response) => {
           this.likedMovies = response; 
       })
   }
}

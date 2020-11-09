import { UserDto } from './user.dto';

export class FavortiesDto {
    likedId: number;
    movieId: number;
    userId: number;
    title: string;
    poster: string;
    dateCreated: string;
    action: string; 

    constructor(favorite: any, user: UserDto, action: string = null) {
        this.userId = user.userId;
        this.movieId = favorite.movieId;
        this.title = favorite.title;
        this.poster = favorite.poster;
        this.likedId = favorite.likedId;
        this.dateCreated = this.dateCreated;
        this.action = action;
    }
}
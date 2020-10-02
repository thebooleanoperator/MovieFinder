import { UserDto } from './user.dto';

export class FavortiesDto {
    likedId: number;
    movieId: number;
    userId: number;
    title: string;
    poster: string;

    constructor(favorite: any, user: UserDto) {
        this.userId = user.userId;
        this.movieId = favorite.movieId;
        this.title = favorite.title;
        this.poster = favorite.poster;
    }
}
import { UserDto } from './user.dto';

export class FavortiesDto {
    likedId: number;
    movieId: number;
    userId: number;

    constructor(favorite: any, user: UserDto) {
        this.movieId = favorite.movieId;
        this.userId = user.userId;
    }
}
import { MovieDto } from './movie.dto';
import { UserDto } from './user.dto';

export class FavortiesDto {
    LikedId: number;
    MovieId: number;
    UserId: number;

    constructor(favorite: any, user: UserDto) {
        this.MovieId = favorite.movieId;
        this.UserId = user.userId;
    }
}
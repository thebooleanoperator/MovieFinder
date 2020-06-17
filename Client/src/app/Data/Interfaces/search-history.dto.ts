import { UserDto } from './user.dto';
import { MovieDto } from './movie.dto';

export class SearchHistoryDto {
    movieId: number;
    userId: number;

    constructor(movie: MovieDto, user: UserDto) {
        this.movieId = movie.movieId,
        this.userId = user.userId
    }
}
import { UserDto } from './user.dto';
import { MovieDto } from './movie.dto';

export class SearchHistoryDto {
    movieId: number;
    userId: number;
    title: string;
    poster: string;

    constructor(movie: MovieDto) {
        this.movieId = movie.movieId,
        this.title = movie.title,
        this.poster = movie.poster
    }
}
import { GenresDto } from './genres.dto';
import { StreamingDataDto } from './streamingData.dto';

export class MovieDto {
    movieId: number;
    title: string;
    year: number;
    director: string;
    runTime: number;
    netflix: number;
    plot: string;
    poster: string;
    imdbRating: number;
    rottenTomatoesRating: number;
    isFavorite: boolean;
    genre: GenresDto;
    streamingData: StreamingDataDto;
}
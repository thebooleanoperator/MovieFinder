import { GenresDto } from './genres.dto';
import { StreamingDataDto } from './streamingData.dto';

export class MovieDto {
    title: string;
    year: number;
    director: string;
    runTime: number;
    netflix: number;
    poster: string;
    imdbRating: number;
    rottenTomatoesRating: number;
    genres: GenresDto;
    streamingData: StreamingDataDto;
}
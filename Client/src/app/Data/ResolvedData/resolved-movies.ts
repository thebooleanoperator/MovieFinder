import { MovieDto } from '../Interfaces/movie.dto';

export class ResolvedMovies {
    constructor(public movies: MovieDto[], public error: any = null){}
}
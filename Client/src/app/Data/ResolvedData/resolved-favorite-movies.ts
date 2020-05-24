import { MovieDto } from '../Interfaces/movie.dto';

export class ResolvedFavoriteMovies {
    constructor(public favoriteMovies: MovieDto[], public error: any = null){}
}
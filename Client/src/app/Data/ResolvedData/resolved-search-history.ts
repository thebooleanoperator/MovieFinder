import { MovieDto } from '../Interfaces/movie.dto';

export class ResolvedSearchHistory {
    constructor(public searchHistory: MovieDto[], public error: any = null){}
}
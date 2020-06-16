import { MovieDto } from './movie.dto';
import { FavortiesDto } from './favorites.dto';

export class MovieDialogDto {
    movie: MovieDto;
    favoriteMovies: FavortiesDto[];
    isFavorite: boolean;
    updateSearchHistory: boolean;
}
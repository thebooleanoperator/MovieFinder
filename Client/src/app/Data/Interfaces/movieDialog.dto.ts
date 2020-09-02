import { MovieDto } from './movie.dto';
import { FavortiesDto } from './favorites.dto';

export class MovieDialogDto {
    isGuest: boolean;
    movie: MovieDto;
    favoriteMovies: FavortiesDto[];
    isFavorite: boolean;
    updateSearchHistory: boolean;
}
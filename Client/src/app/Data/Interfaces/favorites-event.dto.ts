import { FavortiesDto } from './favorites.dto';

export class FavoritesEventDto {
    likedId: number;
    movieId: number;
    userId: number;
    title: string;
    poster: string;
    action: string;

    constructor(favoriteDto: FavortiesDto, action: string) {
        this.likedId = favoriteDto.likedId;
        this.movieId = favoriteDto.movieId;
        this.userId = favoriteDto.userId;
        this.title = favoriteDto.title;
        this.poster = favoriteDto.poster;
        this.action = action;
    }
}
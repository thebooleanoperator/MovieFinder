import { FavortiesDto } from '../Interfaces/favorites.dto';

export class ResolvedFavorites {
    constructor(public favorites: FavortiesDto[], public error: any = null) {}
}
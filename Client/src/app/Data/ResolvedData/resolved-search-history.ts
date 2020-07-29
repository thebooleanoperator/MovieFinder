import { SearchHistoryDto } from '../Interfaces/search-history.dto';

export class ResolvedSearchHistory {
    constructor(public searchHistory: SearchHistoryDto[], public error: any = null){}
}
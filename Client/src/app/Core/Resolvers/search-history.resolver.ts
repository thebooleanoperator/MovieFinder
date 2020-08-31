import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ResolvedSearchHistory } from 'src/app/Data/ResolvedData/resolved-search-history';
import { SearchHistoryService } from '../Services/search-history.service';
import { SearchHistoryDto } from 'src/app/Data/Interfaces/search-history.dto';
import { UserService } from '../Services/user.service';

@Injectable()
export class SearchHistoryResolver implements Resolve<ResolvedSearchHistory> {
    constructor(private _searchHistoryService: SearchHistoryService, private _userService: UserService) {}

    resolve(): Observable<ResolvedSearchHistory> {
        if (this._userService.isGuest()) {
           return of(new ResolvedSearchHistory(null, null));
        }
        else {
            return this._searchHistoryService.getAll(20)
            .pipe(
                map((userSearchHistory: SearchHistoryDto[]) => new ResolvedSearchHistory(userSearchHistory)),
                catchError((error: any) => of(new ResolvedSearchHistory(null, error)))
            ); 
        }
    }
}
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs/internal/Subject';
import { SearchHistoryDto } from 'src/app/Data/Interfaces/search-history.dto';

@Injectable({providedIn:'root'})
export class SearchHistoryService {

    constructor(private http: HttpClient){}

    private _searchHistoryUpdated = new Subject();
    searchHistoryUpdated$ = this._searchHistoryUpdated.asObservable();

    // Event Emitters
    searchHistoryUpdated(searchHistory: SearchHistoryDto) {
        this._searchHistoryUpdated.next(searchHistory);
    }

    // Http Methods
    public create(searchHistory): Observable<any> {
        return this.http.post(`/UserSearchHistory`, searchHistory);
    }      

    public getAll(historyLength: number=null): Observable<any> {
        return this.http.get(`/UserSearchHistory?historyLength=${historyLength}`);
    }
}
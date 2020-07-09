import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({providedIn:'root'})
export class SearchHistoryService {

    constructor(private http: HttpClient){}

    public create(searchHistory): Observable<any> {
        return this.http.post(`/UserSearchHistory`, searchHistory);
    }      
}
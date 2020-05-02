import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({providedIn:'root'})
export class ImdbIdsService {

    constructor(private http: HttpClient)
    {

    }

    public getImdbIdsByTitle(title: string, year:number): Observable<any> {
        return year == null 
            ? this.http.get(`http://localhost:5001/ImdbIds/?title=${title}`)
            : this.http.get(`http://localhost:5001/ImdbIds/?title=${title}&year=${year}`);
    }      
}
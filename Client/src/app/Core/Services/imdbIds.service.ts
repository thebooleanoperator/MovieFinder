import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ToolBarService } from './tool-bar.service';

@Injectable({providedIn:'root'})
export class ImdbIdsService {

    constructor(private http: HttpClient, private _toolBarService: ToolBarService)
    {

    }

    public getImdbIdsByTitle(title: string, year:number = null): Observable<any> {
        if (title) {
            this._toolBarService.isLoading = true;
            return this.http.get(`/ImdbIds/?title=${title}&year=${year}`);
        }
        return of();
    }      
}
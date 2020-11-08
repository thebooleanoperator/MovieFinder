import { Injectable } from "@angular/core";
import { Subject } from 'rxjs';

@Injectable({providedIn: 'root'})

export class RecommendedService {
    private _changeRecommended = new Subject();
    recommendedUpdated$ = this._changeRecommended.asObservable();

    // Event Emitters
    recommendedUpdated(index: number) {
        this._changeRecommended.next(index);
    }
}
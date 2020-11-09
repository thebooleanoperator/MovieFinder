import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';

@Injectable()
export class AppUtilities {
    // Data
    public BaseUrl: string = window.location.origin;
    private _clickEvent = new Subject();    

    // Event Emitters
    clickEvent$ = this._clickEvent.asObservable();
    clickEvent(event: Event) {
        this._clickEvent.next(event);
    }
}
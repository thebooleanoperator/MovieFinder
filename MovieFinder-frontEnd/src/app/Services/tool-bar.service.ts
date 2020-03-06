import { Injectable } from "@angular/core";

@Injectable({providedIn: 'root'})

export class ToolBarService {
    //Data 
    private _isLoading: boolean; 

    get isLoading(): boolean {
        return this._isLoading; 
    }

    set isLoading (bool) {
        this._isLoading = bool; 
    }
}
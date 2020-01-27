import { Injectable } from "@angular/core";

@Injectable({providedIn: 'root'})

export class ToolBarService {
    //Data 
    public visible: boolean;

    get vis() : boolean {
        return this.visible; 
    }

    set vis(show: boolean) {
        this.visible = show;
    }
}
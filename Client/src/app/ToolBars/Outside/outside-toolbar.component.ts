import { Component, Input } from "@angular/core";
import { ToolBarService } from 'src/app/Services/tool-bar.service';

@Component({
    selector: 'outside-toolbar',
    templateUrl: './outside-toolbar.component.html',
    styleUrls: ['./outside-toolbar.component.scss']
})
export class OutsideToolbarComponent {
    constructor(private _toolBarService: ToolBarService)
    {

    }

    //Methods
    getIsLoading(): boolean {
        return this._toolBarService.isLoading; 
    }
}
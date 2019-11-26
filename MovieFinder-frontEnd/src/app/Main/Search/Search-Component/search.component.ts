import { Component, OnInit } from '@angular/core';

@Component({
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent {
    showSearch: boolean = false; 

    toggleSearch(): void {
        this.showSearch = !this.showSearch;
    }
}
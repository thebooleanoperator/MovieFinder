import {MatBottomSheet} from '@angular/material/bottom-sheet';
import { Injectable } from '@angular/core';

@Injectable({providedIn: 'root'})
  export class Settings {
    constructor(private _bottomSheet: MatBottomSheet) {}

    openBottomSheet(): void {
        this._bottomSheet.open(Settings);
      }
  
  }
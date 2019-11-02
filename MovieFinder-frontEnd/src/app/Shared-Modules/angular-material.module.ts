import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatButtonModule} from '@angular/material/button';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatIconModule} from '@angular/material/icon';
import {MatTableModule} from '@angular/material/table';

@NgModule({
  declarations: [
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    MatButtonModule,
    MatCheckboxModule,
    MatToolbarModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatTableModule
  ],
  exports: [
    BrowserAnimationsModule,
    BrowserModule,
    MatButtonModule,
    MatCheckboxModule,
    MatToolbarModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatTableModule
  ],
  providers: [],
  bootstrap: []
})
export class AngularMaterialModule { }

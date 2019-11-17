import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'; 
import { AngularMaterialModule } from 'src/app/Shared-Modules/angular-material.module';
import { SearchComponent } from './Search-Component/search.component';


@NgModule({
  declarations: [
    SearchComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AngularMaterialModule
  ],
  exports: [
    SearchComponent
],
})
export class SearchModule { }
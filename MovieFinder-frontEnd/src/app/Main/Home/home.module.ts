import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'; 

import { AngularMaterialModule } from 'src/app/Shared-Modules/angular-material.module';
import { DashboardComponent } from './Dashboard-Component/dashboard.component';
import { SearchComponent } from './Search-Component/search.component';
import { ToolBarComponent } from 'src/app/ToolBar/tool-bar.component';

@NgModule({
  declarations: [
    ToolBarComponent,
    DashboardComponent,
    SearchComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AngularMaterialModule,
  ]
})
export class HomeModule { }
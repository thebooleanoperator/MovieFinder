import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'; 
import { Routes, RouterModule } from '@angular/router';

import { AngularMaterialModule } from '../Shared-Modules/angular-material.module';
import { WelcomeComponent } from './Welcome/welcome.component';
import { LoginComponent } from './Login/login.component';

@NgModule({
  declarations: [
    WelcomeComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AngularMaterialModule,
    RouterModule
  ]
})
export class OutsideModule { }
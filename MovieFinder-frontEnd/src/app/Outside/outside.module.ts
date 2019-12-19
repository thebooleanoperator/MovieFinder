import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'; 
import { Routes, RouterModule } from '@angular/router';

import { AngularMaterialModule } from '../Shared-Modules/angular-material.module';
import { WelcomeComponent } from './Welcome/welcome.component';
import { LoginComponent } from './Login/login.component';
import { RegisterComponent } from './Register/register.component';

@NgModule({
  declarations: [
    WelcomeComponent,
    RegisterComponent,
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
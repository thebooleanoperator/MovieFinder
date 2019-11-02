import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http'; 

import { AngularMaterialModule } from '../Shared-Modules/angular-material.module';

import { LoginComponent } from './Login/login.component';
import { WelcomeComponent } from './welcome.component';
import { WelcomeRoutingModule } from './welcome-routing.module';

@NgModule({
  declarations: [
    WelcomeComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    WelcomeRoutingModule,
    AngularMaterialModule,
  ],
  exports: [
      WelcomeComponent,
      LoginComponent
  ],
  providers: []
})
export class WelcomeModule { }
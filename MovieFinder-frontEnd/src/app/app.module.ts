import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'; 

import { AngularMaterialModule } from './Shared-Modules/angular-material.module';
import { OutsideModule } from './Outside/outside.module'
import { HomeModule } from './Main/Home/home.module'
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TokenInterceptor } from './Services/token-interceptor';
import { UserService } from './Services/user.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AngularMaterialModule,
    OutsideModule,
    HomeModule,
    AppRoutingModule
  ],
  providers: [
      {
          provide: HTTP_INTERCEPTORS,
          useClass: TokenInterceptor,
          multi: true
      }
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }



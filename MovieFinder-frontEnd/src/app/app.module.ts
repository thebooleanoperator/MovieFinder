//Modules
import { AngularMaterialModule } from './Shared-Modules/angular-material.module';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'; 
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

//Components
import { AppComponent } from './app.component';
import { TokenInterceptor } from './Services/token-interceptor';
import { ToolBarComponent } from './Main/Home/ToolBar/tool-bar.component';
import { WelcomeComponent } from './Outside/Welcome/welcome.component';
import { RegisterComponent } from './Outside/Welcome/Register/register.component';
import { LoginComponent } from './Outside/Welcome/Login/login.component';
import { DashboardComponent } from './Main/Home/Dashboard-Component/dashboard.component';
import { SearchComponent } from './Main/Home/Search-Component/search.component';
import { MoviesComponent } from './Main/Home/Movies-Component/movies.component';


@NgModule({
  declarations: [
    AppComponent,
    WelcomeComponent,
    RegisterComponent,
    LoginComponent,
    ToolBarComponent,
    DashboardComponent,
    SearchComponent,
    MoviesComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AngularMaterialModule,
    AppRoutingModule,
    RouterModule,
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



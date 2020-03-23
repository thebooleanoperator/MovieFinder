//Modules
import { AngularMaterialModule } from './Shared-Modules/angular-material.module';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'; 
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

//Components
import { AppComponent } from './app.component';
import { TokenInterceptor } from './Services/token-interceptor';
import { InsideToolbarComponent } from './ToolBars/Inside/inside-toolbar.component';
import { WelcomeComponent } from './Outside/Welcome/welcome.component';
import { RegisterComponent } from './Outside/Welcome/Register/register.component';
import { LoginComponent } from './Outside/Welcome/Login/login.component';
import { DashboardComponent } from './Main/Home/Dashboard-Component/dashboard.component';
import { MovieFinderComponent } from './Main/Home/Movie-Finder-Component/movie-finder.component';
import { MovieComponent } from './Main/Home/Selectors/Movie/movie.component';
import { SelectedMovieDialog } from './Main/Home/Dashboard-Component/Dialogs/selected-movie.dialog';
import { OutsideToolbarComponent } from './ToolBars/Outside/outside-toolbar.component';


@NgModule({
  declarations: [
    AppComponent,
    WelcomeComponent,
    RegisterComponent,
    LoginComponent,
    InsideToolbarComponent,
    DashboardComponent,
    MovieFinderComponent,
    MovieComponent,
    SelectedMovieDialog,
    OutsideToolbarComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AngularMaterialModule,
    AppRoutingModule,
    RouterModule,
  ],
  entryComponents: [
      SelectedMovieDialog
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



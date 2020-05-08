// Modules
import { AngularMaterialModule } from './Shared/angular-material.module';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'; 
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
// Components
import { AppComponent } from './app.component';
import { OutsideToolbarComponent } from './Core/ToolBars/Outside/outside-toolbar.component';
import { InsideToolbarComponent } from './Core/ToolBars/Inside/inside-toolbar.component';
import { WelcomeComponent } from './Main/Components/Welcome/welcome.component';
import { RegisterComponent } from './Main/Components/Register/register.component';
import { LoginComponent } from './Main/Components/Login/login.component';
import { DashboardComponent } from './Main/Components/Dashboard/dashboard.component';
import { RecommendationsComponent } from './Main/Components/Recommendations/recommendations.component';
import { MovieComponent } from './Main/Components/Movie/movie.component';
import { SelectedMovieDialog } from './Main/Dialogs/Selected-Movie/selected-movie.dialog';
import { SettingsComponent } from './Main/Components/Settings/settings.component';
import { FavoritesComponent } from './Main/Components/Favorites/favorites.component';
// Resolvers
import { MoviesResolver } from './Core/Resolvers/movies.resolver';
import { FavoritesResolver } from './Core/Resolvers/favorites.resolver';
// Interceptors
import { TokenInterceptor } from './Core/Interceptor/token-interceptor';


@NgModule({
  declarations: [
    AppComponent,
    WelcomeComponent,
    RegisterComponent,
    LoginComponent,
    OutsideToolbarComponent,
    InsideToolbarComponent,
    DashboardComponent,
    RecommendationsComponent,
    MovieComponent,
    SelectedMovieDialog,
    SettingsComponent,
    FavoritesComponent
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
      SelectedMovieDialog,
      SettingsComponent
  ],
  providers: [
      {
          provide: HTTP_INTERCEPTORS,
          useClass: TokenInterceptor,
          multi: true
      },
      MoviesResolver,
      FavoritesResolver
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }



// Modules
import { AngularMaterialModule } from './Shared/angular-material.module';
import { AngularLibrariesModule } from './Shared/angular-libraries.module';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'; 
import { NgModule } from '@angular/core';
// Components
import { AppComponent } from './app.component';
import { SelectedMovieDialog } from './Main/Dialogs/Selected-Movie/selected-movie.dialog';
import { SettingsComponent } from './Main/Components/Settings/settings.component';
import { InfinityScrollComponent } from './Main/Components/Infinity-Scroll/infinity-scroll.component';
// Resolvers
import { MoviesResolver } from './Core/Resolvers/movies.resolver';
import { FavoritesResolver } from './Core/Resolvers/favorites.resolver';
import { SearchHistoryResolver } from './Core/Resolvers/search-history.resolver';
// Interceptors
import { TokenInterceptor } from './Core/Interceptor/token-interceptor';
import { ChangePasswordDialog } from './Main/Dialogs/ResetPassword/change-password-dialog';
import { AppUtilities } from './Core/Utilities/app-utilities';
import { WelcomeModule } from './Main/Modules/Welcome/welcome.module';
import { AngularFormsModule } from './Shared/angular-forms.module';
import { ContentModule } from './Main/Modules/Content/content.module';
import { GuestHelpDialog } from './Main/Dialogs/Guest-Help/guest-help-dialog';
import { RecommendationsComponent } from './Main/Components/Recommendations/recommendations.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AngularFormsModule,
    HttpClientModule,
    AngularMaterialModule,
    AngularLibrariesModule,
    WelcomeModule,
    ContentModule,
    AppRoutingModule
  ],
  entryComponents: [
      SelectedMovieDialog,
      ChangePasswordDialog,
      GuestHelpDialog,
      SettingsComponent,
      InfinityScrollComponent,
      RecommendationsComponent
  ],
  providers: [
      {
          provide: HTTP_INTERCEPTORS,
          useClass: TokenInterceptor,
          multi: true
      },
      MoviesResolver,
      FavoritesResolver,
      SearchHistoryResolver,
      AppUtilities
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }

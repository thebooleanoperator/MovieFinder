import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WelcomeComponent } from './Main/Components/Welcome/welcome.component';
import { AuthGuardService } from './Core/Services/auth-guard.service';
import { ContentComponent } from './Main/Components/Content/content.component';
import { DashboardComponent } from './Main/Components/Dashboard/dashboard.component';
import { FavoritesResolver } from './Core/Resolvers/favorites.resolver';
import { SearchHistoryResolver } from './Core/Resolvers/search-history.resolver';
import { FavoritesComponent } from './Main/Components/Favorites/favorites.component';
import { FavoriteMoviesResolver } from './Core/Resolvers/favorite-movies.resolver';
import { RecommendationsComponent } from './Main/Components/Recommendations/recommendations.component';
import { MoviesResolver } from './Core/Resolvers/movies.resolver';

const routes: Routes = [
    {path: 'welcome',  component: WelcomeComponent}, 
    {path: 'content',   component: ContentComponent, canActivate: [AuthGuardService],
     children: [
        {path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuardService], resolve: {
            resolvedFavorites: FavoritesResolver, 
            resolvedSearchHistory: SearchHistoryResolver
        }},
        {path: 'favorites', component: FavoritesComponent, canActivate: [AuthGuardService], resolve: {
            resolvedFavoriteMovies: FavoriteMoviesResolver, 
            resolvedFavorites: FavoritesResolver
        }},
        {path: 'movies', component: RecommendationsComponent, canActivate: [AuthGuardService], resolve: { 
            resolvedMovies: MoviesResolver, 
            resolvedFavorites: FavoritesResolver
        }},
     ]},
    {path: '**', redirectTo:'content'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WelcomeComponent } from './Main/Components/Welcome/welcome.component';
import { AuthGuardService } from './Core/Services/auth-guard.service';
import { ContentComponent } from './Main/Components/Content/content.component';
import { DashboardComponent } from './Main/Components/Dashboard/dashboard.component';
import { FavoritesResolver } from './Core/Resolvers/favorites.resolver';
import { SearchHistoryResolver } from './Core/Resolvers/search-history.resolver';
import { FavoriteMoviesResolver } from './Core/Resolvers/favorite-movies.resolver';
import { MoviesResolver } from './Core/Resolvers/movies.resolver';

const routes: Routes = [
    {path: 'welcome',  component: WelcomeComponent}, 
    {path: 'content',   component: ContentComponent, canActivate: [AuthGuardService],
        children: [
            {
                path: 'dashboard', 
                component: DashboardComponent, 
                canActivate: [AuthGuardService], 
                resolve: {
                    resolvedFavorites: FavoritesResolver, 
                    resolvedFavoriteMovies: FavoriteMoviesResolver, 
                    resolvedSearchHistory: SearchHistoryResolver,
                    resolvedMovies: MoviesResolver
                }
            }
     ]},
    {path: '**', redirectTo:'welcome'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

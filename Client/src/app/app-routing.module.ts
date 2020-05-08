import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WelcomeComponent } from './Main/Components/Welcome/welcome.component';
import { DashboardComponent } from './Main/Components/Dashboard/dashboard.component';
import { RecommendationsComponent } from './Main/Components/Recommendations/recommendations.component';
import { AuthGuardService } from './Core/Services/auth-guard.service';
import { FavoritesComponent } from './Main/Components/Favorites/favorites.component';
import { MoviesResolver } from './Core/Resolvers/movies.resolver';
import { FavoritesResolver } from './Core/Resolvers/favorites.resolver';

const routes: Routes = [
    {path: 'welcome', component: WelcomeComponent},
    {path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuardService]},
    {path: 'favorites', component: FavoritesComponent, canActivate: [AuthGuardService]},
    {path: 'movies', component: RecommendationsComponent, canActivate: [AuthGuardService], resolve: { movies: MoviesResolver, favoriteMovies: FavoritesResolver}},

    {path: '**', redirectTo:'dashboard'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

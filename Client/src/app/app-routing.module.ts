import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WelcomeComponent } from './Outside/Welcome/welcome.component';
import { DashboardComponent } from './Main/Home/Dashboard-Component/dashboard.component';
import { MovieFinderComponent } from './Main/Home/Movie-Finder-Component/movie-finder.component';
import { AuthGuardService } from './Services/auth-guard.service';

const routes: Routes = [
    {path: 'welcome', component: WelcomeComponent},
    {path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuardService]},
    {path: 'movies', component: MovieFinderComponent, canActivate: [AuthGuardService]},

    {path: '**', redirectTo:'welcome'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
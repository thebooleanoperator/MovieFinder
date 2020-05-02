import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WelcomeComponent } from './Main/Components/Welcome/welcome.component';
import { DashboardComponent } from './Main/Components/Dashboard/dashboard.component';
import { MovieFinderComponent } from './Main/Components/Recommendations/movie-finder.component';
import { AuthGuardService } from './Core/Services/auth-guard.service';

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

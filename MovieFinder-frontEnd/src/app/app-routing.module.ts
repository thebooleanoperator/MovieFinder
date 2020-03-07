import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WelcomeComponent } from './Outside/Welcome/welcome.component';
import { LoginComponent } from './Outside/Welcome/Login/login.component';
import { DashboardComponent } from './Main/Home/Dashboard-Component/dashboard.component';
import { RegisterComponent } from './Outside/Welcome/Register/register.component';
import { MovieFinderComponent } from './Main/Home/Movie-Finder-Component/movie-finder.component';
import { AuthGuardService } from './Services/auth-guard.service';

const routes: Routes = [
    {path: 'welcome', component: WelcomeComponent},
    {path: 'login', component: LoginComponent},
    {path: 'register', component: RegisterComponent},
    {path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuardService]},
    {path: 'movies', component: MovieFinderComponent, canActivate: [AuthGuardService]},

    {path: '**', redirectTo:'welcome'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

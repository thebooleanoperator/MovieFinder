import { NgModule } from "@angular/core";
import { DashboardComponent } from '../../Components/Dashboard/dashboard.component';
import { RecommendationsComponent } from '../../Components/Recommendations/recommendations.component';
import { MovieComponent } from '../../Components/Movie/movie.component';
import { SettingsComponent } from '../../Components/Settings/settings.component';
import { SelectedMovieDialog } from '../../Dialogs/Selected-Movie/selected-movie.dialog';
import { ChangePasswordDialog } from '../../Dialogs/ResetPassword/change-password-dialog';
import { BrowserModule } from '@angular/platform-browser';
import { AngularFormsModule } from 'src/app/Shared/angular-forms.module';
import { HttpClientModule } from '@angular/common/http';
import { AngularMaterialModule } from 'src/app/Shared/angular-material.module';
import { AngularLibrariesModule } from 'src/app/Shared/angular-libraries.module';
import { ContentComponent } from '../../Components/Content/content.component';
import { RouterModule } from '@angular/router';
import { InsideToolbarComponent } from 'src/app/Core/ToolBars/Inside/inside-toolbar.component';
import { InfinityScrollComponent } from '../../Components/Infinity-Scroll/infinity-scroll.component';
import { SearchComponent } from '../../Components/Search/search.component';

@NgModule({
    declarations: [
        DashboardComponent,
        InsideToolbarComponent,
        RecommendationsComponent,
        MovieComponent,
        SettingsComponent,
        InfinityScrollComponent,
        SelectedMovieDialog,
        ChangePasswordDialog,
        ContentComponent,
        SearchComponent
    ],
    imports: [
        BrowserModule,
        AngularFormsModule,
        HttpClientModule,
        AngularMaterialModule,
        AngularLibrariesModule,
        RouterModule
    ],
    exports: [
        DashboardComponent,
        InsideToolbarComponent,
        RecommendationsComponent,
        MovieComponent,
        SettingsComponent,
        InfinityScrollComponent,
        SelectedMovieDialog,
        ChangePasswordDialog,
        ContentComponent,
        SearchComponent
    ]
})

export class ContentModule {}
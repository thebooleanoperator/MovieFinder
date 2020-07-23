import { NgModule } from "@angular/core";
import { WelcomeComponent } from '../Components/Welcome/welcome.component';
import { RegisterComponent } from '../Components/Register/register.component';
import { LoginComponent } from '../Components/Login/login.component';
import { OutsideToolbarComponent } from 'src/app/Core/ToolBars/Outside/outside-toolbar.component';
import { HttpClientModule } from '@angular/common/http';
import { AngularMaterialModule } from 'src/app/Shared/angular-material.module';
import { AngularLibrariesModule } from 'src/app/Shared/angular-libraries.module';
import { AppRoutingModule } from 'src/app/app-routing.module';
import { RouterModule } from '@angular/router';
import { AngularFormsModule } from 'src/app/Shared/angular-forms.module';
import { BrowserModule } from '@angular/platform-browser';

@NgModule ({
    declarations: [
        WelcomeComponent,
        RegisterComponent,
        LoginComponent,
        OutsideToolbarComponent
    ],
    imports: [
        BrowserModule,
        AngularFormsModule,
        HttpClientModule,
        AngularMaterialModule,
        AngularLibrariesModule,
        AppRoutingModule,
        RouterModule
      ],
    exports: [
        WelcomeComponent,
        RegisterComponent,
        LoginComponent,
        OutsideToolbarComponent
    ]
})

export class AuthenticationModule {}
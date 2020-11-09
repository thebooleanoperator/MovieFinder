import { NgModule } from "@angular/core";
import { WelcomeComponent } from '../../Components/Welcome/welcome.component';
import { RegisterComponent } from '../../Components/Register/register.component';
import { LoginComponent } from '../../Components/Login/login.component';
import { HttpClientModule } from '@angular/common/http';
import { AngularMaterialModule } from 'src/app/Shared/angular-material.module';
import { AngularLibrariesModule } from 'src/app/Shared/angular-libraries.module';
import { AngularFormsModule } from 'src/app/Shared/angular-forms.module';
import { BrowserModule } from '@angular/platform-browser';
import { GuestHelpDialog } from '../../Dialogs/Guest-Help/guest-help-dialog';

@NgModule ({
    declarations: [
        WelcomeComponent,
        RegisterComponent,
        LoginComponent,
        GuestHelpDialog
    ],
    imports: [
        BrowserModule,
        AngularFormsModule,
        HttpClientModule,
        AngularMaterialModule,
        AngularLibrariesModule,
      ],
    exports: [
        WelcomeComponent,
        RegisterComponent,
        LoginComponent,
        GuestHelpDialog
    ]
})

export class WelcomeModule {}
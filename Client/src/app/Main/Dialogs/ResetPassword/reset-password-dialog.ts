import { Component } from "@angular/core";
import { MatDialogRef } from '@angular/material/dialog/typings';

@Component({
    selector: 'reset-password-dialog',
    templateUrl: './reset-password.html',
    styleUrls: ['./reset-password.scss']
})
export class ResetPasswordDialog {
    constructor(public dialogRef: MatDialogRef<ResetPasswordDialog>){}

    
}   
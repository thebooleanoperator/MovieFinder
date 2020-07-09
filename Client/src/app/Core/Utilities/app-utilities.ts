import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class AppUtilities {
    // Data
    public BaseUrl: string = window.location.origin;
}
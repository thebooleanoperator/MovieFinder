import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MovieDto } from 'src/app/Data/movie.dto';
import { MovieDialogDto } from 'src/app/Data/movieDialog.dto';

@Component ({
    selector: "selected-movie-dialog",
    templateUrl: "./selected-movie.dialog.html"
})
export class SelectedMovieDialog {
    constructor(public dialogRef: MatDialogRef<SelectedMovieDialog>, @Inject(MAT_DIALOG_DATA) public data: MovieDialogDto) 
    {
        
    }
    
    showMovie: boolean = false;
    movieDto: MovieDto = this.data.movieDto;

    onNoClick(): void {
        this.dialogRef.close();
    }
}
import { Component } from '@angular/core';
import { MovieDto } from 'src/app/Data/Interfaces/movie.dto';
import { MatDialog } from '@angular/material/dialog';
import { SelectedMovieDialog } from '../../Dialogs/Selected-Movie/selected-movie.dialog';
import { DialogWatcherService } from 'src/app/Core/Services/dialog-watcher.service';

@Component({
    selector: 'search-history',
    templateUrl: './search-history.component.html',
    styleUrls: ['./search-history.component.scss']
})
export class SearchHistoryComponent {
    constructor(
        private _dialog: MatDialog,
        private _dialogWatcher: DialogWatcherService
    ){}
    searchedMovies: MovieDto[];
}
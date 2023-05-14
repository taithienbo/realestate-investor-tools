import { Component, EventEmitter, Output } from '@angular/core';
import { DataService } from '../core/services/data.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss'],
})
export class SearchComponent {
  searchQuery: string = '';

  constructor(private dataService: DataService) {}

  onSearchButtonClick() {
    console.log('onsearchButtonClick() called.');
    this.dataService.setSearchQuery(this.searchQuery);
  }
}

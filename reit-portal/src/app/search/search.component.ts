import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss'],
})
export class SearchComponent {
  @Output()
  searchQueryEmitter = new EventEmitter<string>();

  searchQuery: string = '';

  onSearchButtonClick() {
    this.searchQueryEmitter.emit(this.searchQuery);
  }
}

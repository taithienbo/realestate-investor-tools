import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchComponent } from './search.component';
import { FormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { DataService } from '../core/services/data.service';

class MockDataService {
  listingDetailObservable$ = {
    subscribe: () => {},
  };
  setListingDetail() {}
  setSearchQuery(searchQuery: string) {}
}

describe('SearchComponent', () => {
  let component: SearchComponent;
  let fixture: ComponentFixture<SearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [{ provide: DataService, useClass: MockDataService }],
      declarations: [SearchComponent],
      imports: [FormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(SearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have a search bar', () => {
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.search-bar')).toBeTruthy();
  });

  it('should have a search button', () => {
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.search-button')).toBeTruthy();
  });

  it('should update searchQuery when user enters text in search box', () => {
    const searchInput = fixture.debugElement.query(
      By.css('input[type="search"]')
    ).nativeElement;

    searchInput.value = 'search text'; // Set the value of the input element
    searchInput.dispatchEvent(new Event('input')); // Trigger the input event

    expect(component.searchQuery).toEqual('search text');
  });

  it('should update search query in data store when search button is clicked', () => {
    const searchInput = fixture.debugElement.query(
      By.css('input[type="search"]')
    ).nativeElement;

    searchInput.value = 'search text'; // Set the value of the input element
    searchInput.dispatchEvent(new Event('input')); // Trigger the input event

    // validate the component calls a method of data service
    const dataService = TestBed.inject(DataService);
    spyOn(dataService, 'setSearchQuery');
    fixture.nativeElement.querySelector('.search-button').click();
    expect(dataService.setSearchQuery).toHaveBeenCalledWith('search text');
  });
});

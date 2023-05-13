import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AppComponent } from './app.component';
import { NavigationComponent } from './core/navigation/navigation.component';
import { SearchComponent } from './search/search.component';
import { ToolbarComponent } from './shared/tool-bar/tool-bar.component';
import { Component, EventEmitter, Output } from '@angular/core';

// create a test search component to mock the search component
@Component({
  selector: 'app-search',
  template:
    '<button class="search-button" (click)="onSearchButtonClick()"></button>',
})
class MockSearchComponent {
  @Output()
  searchQueryEmitter = new EventEmitter<string>();
  searchQuery: string = '';
  onSearchButtonClick() {
    this.searchQueryEmitter.emit(this.searchQuery);
  }
}

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      providers: [MockSearchComponent],
      declarations: [
        AppComponent,
        NavigationComponent,
        MockSearchComponent,
        ToolbarComponent,
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'reit-portal'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('reit-portal');
  });

  it('should receive search query from search component', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;

    // spy on app.onSearch to make sure it's called later
    const searchQuery = 'search query';
    // retrieve the mock search component to trigger the search event
    const searchComponent =
      fixture.debugElement.injector.get(MockSearchComponent);

    searchComponent.searchQuery = searchQuery;

    spyOn(app, 'onSearch');
    // trigger the search event by clicking on the Search button
    fixture.nativeElement.querySelector('.search-button').click();

    expect(app.onSearch).toHaveBeenCalled();
  });
});

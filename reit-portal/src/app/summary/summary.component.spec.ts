import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SummaryComponent } from './summary.component';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { IListingDetail } from '../models/listing-detail';
import { BehaviorSubject, of } from 'rxjs';
import { DataService } from '../core/services/data.service';
import { ApiService } from '../core/services/api.service';

class MockApiService {
  getListingDetails() {
    return {
      numOfBathrooms: 2,
      numOfBedrooms: 3,
      numOfGarageSpaces: 2,
      hasHOA: false,
    };
  }
}

class MockDataService {
  private searchQuery$: BehaviorSubject<string | undefined> =
    new BehaviorSubject<string | undefined>(undefined);

  get listingDetailObservable$() {
    return of({
      numOfBathrooms: 2,
      numOfBedrooms: 3,
      numOfGarageSpaces: 2,
      hasHOA: false,
    });
  }

  get searchQueryObservables$() {
    return of('test');
  }

  setSearchQuery(searchQuery: string) {
    console.log('setSearchQuery() called');
    this.searchQuery$.next(searchQuery);
  }
}

describe('SummaryComponent', () => {
  let component: SummaryComponent;
  let fixture: ComponentFixture<SummaryComponent>;

  const listingModel: IListingDetail = {
    numOfBathrooms: 2,
    numOfBedrooms: 3,
    numOfGarageSpaces: 2,
    hasHOA: false,
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SummaryComponent],
      providers: [
        { provide: ApiService, useClass: MockApiService },
        { provide: DataService, useClass: MockDataService },
      ],
      imports: [HttpClientTestingModule],
    }).compileComponents();

    fixture = TestBed.createComponent(SummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('call api service to retrieve listing details when search query changes', () => {
    const apiService = TestBed.inject(ApiService);
    spyOn(apiService, 'getListingDetails').and.callThrough();

    const dataService = TestBed.inject(DataService);
    dataService.setSearchQuery('test');

    expect(apiService.getListingDetails).toHaveBeenCalled();
  });

  it('should have listing detail model as input', () => {
    let listingDetail: IListingDetail = {
      numOfBathrooms: 2,
      numOfBedrooms: 3,
      hasHOA: false,
    };
    component.listingDetail = listingDetail;
    expect(component.listingDetail).toBeTruthy();
  });

  it('should display number of bedrooms', () => {
    component.listingDetail = listingModel;
    fixture.detectChanges();
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.listing-info').textContent).toContain(
      `${listingModel.numOfBedrooms} bedrooms`
    );
    expect(compiled.querySelector('.listing-info').textContent).toContain(
      `${listingModel.numOfBathrooms} bathrooms`
    );
  });
});

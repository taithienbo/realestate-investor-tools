import {
  ComponentFixture,
  TestBed,
  fakeAsync,
  tick,
} from '@angular/core/testing';

import { SummaryComponent } from './summary.component';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { IListingDetail } from '../models/listing-detail';
import { BehaviorSubject, of } from 'rxjs';
import { DataService } from '../core/services/data.service';
import { ApiService } from '../core/services/api.service';

class MockApiService {
  getListingDetails() {
    console.log(`MockApiService#getListingDetails() called.`);
    return of({
      numOfBathrooms: 2,
      numOfBedrooms: 3,
      numOfGarageSpaces: 2,
      hasHOA: false,
    });
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
    return this.searchQuery$.asObservable();
  }

  setSearchQuery(searchQuery: string) {
    console.log(`setSearchQuery() called. Emitting ${searchQuery}`);
    this.searchQuery$.next(searchQuery);
  }

  setListingDetail(listingDetail: IListingDetail) {
    console.log('MockDataService#setListingDetail() called');
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

  it('call api service to retrieve listing details when search query changes', fakeAsync(() => {
    const apiService = TestBed.inject(ApiService);
    spyOn(apiService, 'getListingDetails').and.callThrough();

    const dataService = TestBed.inject(DataService);
    dataService.setSearchQuery('test');
    // wait for the observable to emit
    tick();
    expect(apiService.getListingDetails).toHaveBeenCalled();
  }));

  it('should have listing detail model as input', () => {
    let listingDetail: IListingDetail = {
      numOfBathrooms: 2,
      numOfBedrooms: 3,
      hasHOA: false,
    };
    component.listingDetail = listingDetail;
    expect(component.listingDetail).toBeTruthy();
  });

  it('hide listing detail when listing detail model is undefined', () => {
    component.listingDetail = undefined;
    fixture.detectChanges();
    const compiled = fixture.nativeElement;
    expect(compiled.querySelector('.listing-info')).toBeFalsy();
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

  it('calls API to retrieve listing details and save to data store when search query changes', fakeAsync(() => {
    const dataService = TestBed.inject(DataService);
    spyOn(dataService, 'setListingDetail');

    const apiService = TestBed.inject(ApiService);
    spyOn(apiService, 'getListingDetails').and.callThrough();

    dataService.setSearchQuery('test');
    tick();
    expect(apiService.getListingDetails).toHaveBeenCalled();
    expect(dataService.setListingDetail).toHaveBeenCalled();
  }));
});

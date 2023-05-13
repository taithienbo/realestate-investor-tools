import { TestBed } from '@angular/core/testing';

import { DataService } from './data.service';

describe('DataService', () => {
  let service: DataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should have listingDetailObservable$ to contain listing details', () => {
    expect(service.listingDetailObservable$).toBeTruthy();
  });

  it('should have set listingDetail method', () => {
    expect(service.setListingDetail).toBeTruthy();
  });

  it('should have searchQueryObservables$ to contain search query', () => {
    expect(service.searchQueryObservables$).toBeTruthy();
  });

  it('should have setSearchQuery method', () => {
    expect(service.setSearchQuery).toBeTruthy();
  });
});

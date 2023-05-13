import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { IListingDetail } from 'src/app/models/listing-detail';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  private listingDetail$: BehaviorSubject<IListingDetail | undefined> =
    new BehaviorSubject<IListingDetail | undefined>({});

  private searchQuery$: BehaviorSubject<string | undefined> =
    new BehaviorSubject<string | undefined>(undefined);

  constructor() {}

  get listingDetailObservable$() {
    return this.listingDetail$.asObservable();
  }

  setListingDetail(listingDetail: IListingDetail) {
    this.listingDetail$.next(listingDetail);
  }

  get searchQueryObservables$() {
    return this.searchQuery$.asObservable();
  }

  setSearchQuery(searchQuery: string) {
    this.searchQuery$.next(searchQuery);
  }
}

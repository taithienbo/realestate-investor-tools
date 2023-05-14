import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ApiService } from '../core/services/api.service';
import { IListingDetail } from '../models/listing-detail';
import { DataService } from '../core/services/data.service';
import { EMPTY, Subject, switchMap, takeUntil } from 'rxjs';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.scss'],
})
export class SummaryComponent implements OnInit, OnDestroy {
  constructor(
    private apiService: ApiService,
    private dataService: DataService
  ) {
    console.log('Subscribing to searchQueryObservables$');
  }

  private destroy$ = new Subject<void>();

  @Input()
  listingDetail?: IListingDetail;

  ngOnInit() {
    this.monitorSearchQuery();
    this.monitorListingDetail();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private monitorListingDetail() {
    this.dataService.listingDetailObservable$
      .pipe(takeUntil(this.destroy$))
      .subscribe((listingDetail) => {
        this.listingDetail = listingDetail;
      });
  }

  private monitorSearchQuery() {
    this.dataService.searchQueryObservables$
      .pipe(
        switchMap((searchQuery) => {
          console.log(
            `SummaryComponent#ngOnInit called. SearchQuery: ${searchQuery}`
          );
          if (searchQuery) {
            return this.apiService.getListingDetails(searchQuery);
          }
          return EMPTY;
        }),
        takeUntil(this.destroy$)
      )
      .subscribe((listingDetail) => {
        this.dataService.setListingDetail(listingDetail);
      });
  }
}

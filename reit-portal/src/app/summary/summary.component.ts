import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ApiService } from '../core/services/api.service';
import { IListingDetail } from '../models/listing-detail';
import { DataService } from '../core/services/data.service';
import { Subject, takeUntil } from 'rxjs';

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
    this.dataService.searchQueryObservables$
      .pipe(takeUntil(this.destroy$))
      .subscribe((searchQuery) => {
        if (searchQuery) {
          this.apiService.getListingDetails(searchQuery);
        }
      });
  }

  private destroy$ = new Subject<void>();

  @Input()
  listingDetail?: IListingDetail;

  ngOnInit() {}

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}

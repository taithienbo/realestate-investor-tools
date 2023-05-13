import { Component, Input, OnInit } from '@angular/core';
import { AppService } from '../core/services/app.service';
import { IListingDetail } from '../models/listing-detail';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.scss'],
})
export class SummaryComponent implements OnInit {
  constructor(private appService: AppService) {}

  @Input()
  listingDetail?: IListingDetail;

  ngOnInit() {
    this.appService.getData().subscribe((data) => {
      console.log(data);
    });
  }
}

import { Component, OnInit } from '@angular/core';
import { AppService } from '../core/services/app.service';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.scss'],
})
export class SummaryComponent implements OnInit {
  constructor(private appService: AppService) {}

  ngOnInit() {
    this.appService.getData().subscribe((data) => {
      console.log(data);
    });
  }
}

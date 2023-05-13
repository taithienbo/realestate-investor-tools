import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SummaryComponent } from './summary.component';
import { HttpClientModule } from '@angular/common/http';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { IListingDetail } from '../models/listing-detail';

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
      imports: [HttpClientTestingModule],
    }).compileComponents();

    fixture = TestBed.createComponent(SummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
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

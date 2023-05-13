import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  // create a constructor with http client for calling api
  constructor(private http: HttpClient) {
    // configure http client with base url
  }

  // create a method to get data from api
  getData() {
    let url = './api/PropertiesAnalyzer/address';
    // create query parameters to add to the URL
    let params = {
      address: '5411-Christal-Ave-Garden-Grove-CA-92845',
    };
    return this.http.get(url, { params: params });
  }

  getListingDetails(searchQuery: string) {
    return {
      numOfBathrooms: 2,
      numOfBedrooms: 3,
      numOfGarageSpaces: 2,
      hasHOA: false,
    };
  }
}

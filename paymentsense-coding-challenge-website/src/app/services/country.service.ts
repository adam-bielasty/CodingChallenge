import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PagedListResponse } from '../models/PagedListResponse';
import { Country } from '../models/country';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CountryService {

  constructor(private httpClient: HttpClient) {}

  getPagedCountries(page: number, searchText: string): Observable<PagedListResponse<Country>> {
    return this.httpClient.get<PagedListResponse<Country>>(`https://localhost:44341/PaymentsenseCodingChallenge/countries/${page}?searchText=${encodeURIComponent(searchText)}`);
  }

  add(name: string): Observable<Country> {
    // Task 5) just adding name for simplicity
    return this.httpClient.post<Country>('https://localhost:44341/PaymentsenseCodingChallenge/countries', <Country>{ 
      name: name, 
      languages: [],
      timeZones: [],
      borders: [],
      countryBorders: [],
      currencies: []
     });
  }

}

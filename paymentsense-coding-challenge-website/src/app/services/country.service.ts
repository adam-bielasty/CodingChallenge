import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PagedListResponse } from '../models/PagedListResponse';
import { Country } from '../models/country';
import { Observable } from 'rxjs';
import { Language } from '../models/Language';

@Injectable({
  providedIn: 'root'
})
export class CountryService {

  constructor(private httpClient: HttpClient) {}

  public getPagedCountries(page: number): Observable<PagedListResponse<Country>> {
    return this.httpClient.get<PagedListResponse<Country>>(`https://localhost:44341/PaymentsenseCodingChallenge/countries/${page}`);
  }
}

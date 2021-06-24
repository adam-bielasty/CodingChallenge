import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RestCountryService {

  constructor(private httpClient: HttpClient) {}

  public get(): Observable<string> {
    return this.httpClient.get('https://localhost:44341/health', { responseType: 'text' });
  }
}

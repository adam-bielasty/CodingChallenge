import { Component, OnInit, Input } from '@angular/core';
import { Country } from 'src/app/models/country';

@Component({
  selector: 'app-country',
  templateUrl: './country.component.html',
  styleUrls: ['./country.component.scss']
})
export class CountryComponent implements OnInit {

  @Input() country: Country;
  showDetails: boolean = false;
  displayLanguageNames: string;
  displayCurrencyNames: string;
  displayBorderNames: string;

  constructor() { }

  ngOnInit() {
    if (this.country) {
      if (this.country.languages) {
        this.displayLanguageNames = this.country.languages.map(x => x.name).join(", ");
      }
      if (this.country.currencies) {
        this.displayCurrencyNames = this.country.currencies.map(x => x.name).join(", ");
      }
      if (this.country.countryBorders) {
        this.displayBorderNames = this.country.countryBorders.join(", ");
      }
    }

  }

  toggleDetails(country: Country) {
    this.showDetails = !this.showDetails;
  }

}

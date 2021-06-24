import { Component, OnInit, Input } from '@angular/core';
import { Country } from 'src/app/models/country';
import { CountryService } from 'src/app/services';
import { MetaData } from 'src/app/models/MetaData';

@Component({
  selector: 'app-country-list',
  templateUrl: './country-list.component.html',
  styleUrls: ['./country-list.component.scss']
})
export class CountryListComponent implements OnInit {

  @Input() page: number = 1;

  countries: Country[];
  metaData: MetaData;

  constructor(private countryService: CountryService) { }

  async ngOnInit() {
    await this.refreshPage(this.page);
  }

  async changePage(page: number) {
    await this.refreshPage(page);
  }

  async refreshPage(page: number) {
    let pagedResult = await this.countryService.getPagedCountries(page).toPromise();
    this.countries = pagedResult.items;
    this.metaData = pagedResult.metaData;
  }

}

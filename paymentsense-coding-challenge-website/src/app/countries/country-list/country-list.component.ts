import { Component, OnInit, Input, OnChanges } from '@angular/core';
import { Country } from 'src/app/models/country';
import { CountryService } from 'src/app/services';
import { MetaData } from 'src/app/models/MetaData';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-country-list',
  templateUrl: './country-list.component.html',
  styleUrls: ['./country-list.component.scss']
})
export class CountryListComponent implements OnInit {

  page: number = 1;
  searchText: string = "";
  countries: Country[];
  metaData: MetaData;

  constructor(
    private countryService: CountryService,
    private route: ActivatedRoute,
    private router: Router
    ) { }

  async ngOnInit() {
    this.route.queryParams
    .subscribe(async params => {
      this.page = +params['page'];
      this.searchText = params['searchText'] || "";

      await this.refreshPage(this.page, this.searchText);
    });
  }

  async changePage(page: number) {
    this.router.navigate( ['.'],  { queryParams: { page: page, searchText: this.searchText } })
  }

  async refreshPage(page: number, searchText: string) {
    let pagedResult = await this.countryService.getPagedCountries(page, searchText).toPromise();
    this.countries = pagedResult.items;
    this.metaData = pagedResult.metaData;
  }

}

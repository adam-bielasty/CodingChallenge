import { Component, OnInit, Input } from '@angular/core';
import { Country } from 'src/app/models/country';

@Component({
  selector: 'app-country',
  templateUrl: './country.component.html',
  styleUrls: ['./country.component.scss']
})
export class CountryComponent implements OnInit {

  @Input() country: Country;

  constructor() { }

  ngOnInit() {
  }

}

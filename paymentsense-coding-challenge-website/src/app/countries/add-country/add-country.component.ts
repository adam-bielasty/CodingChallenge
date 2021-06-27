import { Component, OnInit } from '@angular/core';
import { CountryService } from 'src/app/services';

@Component({
  selector: 'app-add-country',
  templateUrl: './add-country.component.html',
  styleUrls: ['./add-country.component.scss']
})
export class AddCountryComponent implements OnInit {

  name: string;
  error: string = null;

  constructor(private countryService: CountryService) { }

  ngOnInit() {
  }

  onSubmit() {
    
    this.countryService
        .add(this.name)
        .subscribe(
            () => {},
            error => {
                if (error.error) {
                    this.error = error.error;
                } else {
                    this.error = 'There has been an error. Check if your inputs are correct.';
                }
            });
  }

}

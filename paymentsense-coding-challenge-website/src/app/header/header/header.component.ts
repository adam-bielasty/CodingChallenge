import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { faSearch, faThumbsUp, faThumbsDown, faTimesCircle } from '@fortawesome/free-solid-svg-icons';
import { NgModule} from '@angular/core'
import { NgForm } from '@angular/forms';
import { PaymentsenseCodingChallengeApiService } from 'src/app/services';
import { take } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  faSearch = faSearch;
  faTimesCircle = faTimesCircle;
  searchText: string;
  
  faThumbsUp = faThumbsUp;
  faThumbsDown = faThumbsDown;
  paymentsenseCodingChallengeApiIsActive = false;
  paymentsenseCodingChallengeApiActiveIcon = this.faThumbsDown;
  paymentsenseCodingChallengeApiActiveIconColour = 'red';

  constructor(
    private paymentsenseCodingChallengeApiService: PaymentsenseCodingChallengeApiService,
    private route: ActivatedRoute,
    private router: Router) {
    paymentsenseCodingChallengeApiService.getHealth().pipe(take(1))
    .subscribe(
      apiHealth => {
        this.paymentsenseCodingChallengeApiIsActive = apiHealth === 'Healthy';
        this.paymentsenseCodingChallengeApiActiveIcon = this.paymentsenseCodingChallengeApiIsActive
          ? this.faThumbsUp
          : this.faThumbsUp;
        this.paymentsenseCodingChallengeApiActiveIconColour = this.paymentsenseCodingChallengeApiIsActive
          ? 'green'
          : 'red';
      },
      _ => {
        this.paymentsenseCodingChallengeApiIsActive = false;
        this.paymentsenseCodingChallengeApiActiveIcon = this.faThumbsDown;
        this.paymentsenseCodingChallengeApiActiveIconColour = 'red';
      });
  }

  ngOnInit() {
    this.route.queryParams
    .subscribe(async params => {
      this.searchText = params['searchText'];
    });
  }

  onSubmit() {
    this.router.navigate( ['.'],  { queryParams: { page: 1, searchText: this.searchText } })
  }

  clearFilter() {
    
    this.router.navigate( ['.'],  { queryParams: { page: 1, searchText: "" } })
  }
}

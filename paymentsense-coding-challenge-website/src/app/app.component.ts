import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  public searchText = "";

  constructor() {
  }

  search(searchText: string) {
    this.searchText = searchText;
  }
}

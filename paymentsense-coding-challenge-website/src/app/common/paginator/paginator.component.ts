import { Component, OnInit, Input, OnChanges, EventEmitter, Output } from '@angular/core';
import { MetaData } from 'src/app/models/MetaData';

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrls: ['./paginator.component.scss']
})
export class PaginatorComponent implements OnInit, OnChanges {

  @Input() metaData: MetaData;
  @Output() changePage = new EventEmitter<number>(true);

  pages: number[] = [];

  constructor() { }

  ngOnInit() {
  }

  ngOnChanges() {
    if (this.metaData && this.metaData.pageCount) {
      this.pages = this.range(1, this.metaData.pageCount + 1, 1)
        .filter(x => (x >= this.metaData.pageNumber-3 && x <= this.metaData.pageNumber + 3));
    }
  }

  setPage(page: number) {
    this.changePage.emit(page);
  }

  // It could go to some common Utils class
  range = (start, end, step = 1) => {
    let output = [];
    if (typeof end === 'undefined') {
      end = start;
      start = 0;
    }
    for (let i = start; i < end; i += step) {
      output.push(i);
    }
    return output;
  };
}

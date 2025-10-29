import { Component, inject, signal } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { AsyncPipe, CommonModule, DatePipe, DecimalPipe, NgFor, NgIf } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatIconModule } from '@angular/material/icon';


@Component({
  selector: 'app-history',
  standalone: true,
  imports: [CommonModule, AsyncPipe, DatePipe, DecimalPipe, MatTableModule, MatPaginatorModule, MatIconModule],
  templateUrl: './history.component.html'
})
export class HistoryComponent {
  private api = inject(ApiService);
  data = signal<any[]>([]);
  total = signal<number>(0);
  pageSize = 5;


  displayedColumns = ['date', 'pair', 'amounts', 'rate'];


  ngOnInit() { this.loadPage(0, this.pageSize); }


  loadPage(skip: number, take: number) {
    this.api.loadHistory({ skip, take, sort: 'desc', userId: 'candidate' })
      .subscribe(result => {
        this.data.set(result.items);
        this.total.set(result.count);
      });
  }


  page(evt: PageEvent) {
    const skip = evt.pageIndex * evt.pageSize;
    console.log(`Loading page ${evt.pageIndex + 1} with page size ${evt.pageSize}`);
    this.pageSize = evt.pageSize;
    this.loadPage(skip, evt.pageSize);
  }

}
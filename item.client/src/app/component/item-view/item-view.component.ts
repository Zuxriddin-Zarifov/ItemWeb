import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Item } from '../../services/models/item';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { ItemService } from '../../services/item-service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-item-view',
  imports: [CommonModule, RouterModule],
  templateUrl: './item-view.component.html',
  styleUrl: './item-view.component.scss'
})
export class ItemViewComponent implements OnInit {

  private route: ActivatedRoute = inject(ActivatedRoute);
  private router: Router = inject(Router);
  private itemService: ItemService = inject(ItemService);

  public TotalItems: number = 0;
  public PageSize: number = Number(localStorage.getItem('pageSize')) || 10;
  public CurrentPage: number = 1;
  public TotalPages: number = 0;
  public Pages: number[] = [];
  public Items: Item[] = [];
  public orderBy: string = "ItemId"
  public isAscending: boolean = true;

  public ngOnInit(): void {
    const expiryTime = localStorage.getItem('expiryTime');
    if (expiryTime && new Date().getTime() > parseInt(expiryTime, 10)) {
      // If the token has expired, clear it and redirect to login
      localStorage.removeItem('authToken');
      localStorage.removeItem('expiryTime');
      this.router.navigate(['/login']);
      return;
    }
    this.route.queryParamMap.subscribe(params => {
      this.CurrentPage = parseInt(params.get('page') || '1', 10);
      if (Math.ceil(this.TotalItems / this.PageSize) === 0) {
        this.CurrentPage = 1;
      }
      this.PageSize = parseInt(params.get('pageSize') || '10', 10);
      this.orderBy = params.get('orderBy') || 'Name';
      this.isAscending = params.get('asc') === 'true';

      this.loadItems(this.CurrentPage);
    });
  }
  public onPageSizeChange(event: Event): void {
    const selectedValue = (event.target as HTMLSelectElement).value;
    this.PageSize = parseInt(selectedValue, 10);

    localStorage.setItem('pageSize', this.PageSize.toString());

    this.CurrentPage = 1;
    this.loadItems(this.CurrentPage);
  }
  public sort(sortItem: string) {
    this.orderBy = sortItem;
    this.isAscending = !this.isAscending;
    this.loadItems(this.CurrentPage);

  }
  public current() {
    this.CurrentPage = 1;
    this.loadItems(this.CurrentPage)
  }
  public next() {
    const maxPage = Math.ceil(this.TotalItems / this.PageSize);
    if (this.CurrentPage < maxPage) {
      this.CurrentPage += 1;
      this.loadItems(this.CurrentPage);
    }
  } 
  public previous() {
    this.CurrentPage = this.CurrentPage - 1 > 0 ? this.CurrentPage - 1 : this.CurrentPage;
    this.loadItems(this.CurrentPage)
  }
  public page(page: number) {
    this.CurrentPage = page;
    this.loadItems(this.CurrentPage);
  }
  public end() {
    this.CurrentPage = Math.ceil(this.TotalItems / this.PageSize);
    this.loadItems(this.CurrentPage)
  }
  public loadItems(page: number): void {
    Swal.fire({
      title: 'Yuklanmoqda...',
      didOpen: () => {
        Swal.showLoading();
      },
      allowOutsideClick: false,
    });

    this.itemService.itemApiService.getItems(page, this.PageSize, this.isAscending, this.orderBy).subscribe({
      next: (response) => {
        this.Items = response.items.map((item: any) =>
          new Item(item.itemId, item.name, item.type, item.date)
        );
        this.TotalItems = response.total;
        this.TotalPages = Math.ceil(this.TotalItems / this.PageSize);
        this.Pages = this.pageFullArray();

        Swal.close()
      },
      error: (error) => {
        console.error('Failed to load items', error);
        Swal.close();
        Swal.fire('Xatolik', 'Ma\'lumotlarni yuklashda muammo yuz berdi', 'error');
      }
    });

    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        page: this.CurrentPage,
        pageSize: this.PageSize,
        orderBy: this.orderBy,
        asc: this.isAscending
      }
    });
  }
  private pageFullArray(): number[] {
    const pages: number[] = [];
    let start = Math.max(1, this.CurrentPage - Math.floor(this.PageSize / 2));
    let end = start + 8;

    if (end > this.TotalPages) {
      end = this.TotalPages;
      start = Math.max(1, end - this.PageSize + 1);
    }
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  }
  public addItem() {
    this.router.navigate(['/item-add']);
  }
  public editItem(id: number) {
    this.router.navigate(['/item-edit'], { queryParams: { id: id } });
  }
  public deleteItem(id: number) {
    this.itemService.itemApiService.deleteItem(id).subscribe(() => {
      this.Items = this.Items.filter(item => item.id !== id);
    });
    this.loadItems(this.CurrentPage);
  }
  public confirmDelete(id: number): void {
    Swal.fire({
      title: 'Delete item?',
      text: 'You won\'t be able to revert this!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes!',
      cancelButtonText: 'No'
    }).then((result) => {
      if (result.isConfirmed) {
        this.deleteItem(id);

        Swal.fire(
          'Deleted!',
          'Your item has been deleted.',
          'success'
        );
      }
    });

  }
}

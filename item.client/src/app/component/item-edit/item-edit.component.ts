import { Component, inject, OnInit} from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ItemApiService } from '../../api/Item-api-service';
import { Item } from '../../services/models/item';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-item-edit',
  imports: [CommonModule],
  templateUrl: './item-edit.component.html',
  styleUrl: './item-edit.component.scss'
})
export class ItemEditComponent implements OnInit {

  public id: number = -1;
  private itemapiService: ItemApiService = inject(ItemApiService);
  private router: Router = inject(Router);
  private route: ActivatedRoute = inject(ActivatedRoute);

  public ngOnInit(): void {
   this.route.queryParams.subscribe(params => {
      this.id = params['id'];
    }); // Initialize the component, if needed
  }
  public saveItem() {
    const nameInput = document.getElementById('name') as HTMLInputElement;
    const typeInput = document.getElementById('type') as HTMLInputElement;
    const dateInput = document.getElementById('date') as HTMLInputElement;
   

    var itemDataAdd = new Item(this.id, nameInput.value, Number(typeInput.value), dateInput.value);

    const observable = this.id === -1 || this.id === undefined
      ? this.itemapiService.addItem(itemDataAdd)
      : this.itemapiService.updateItem(itemDataAdd);
    observable.subscribe({
      next: (response) => {
        Swal.fire({
          icon: 'success',
          title: 'Muvaffaqiyatli',
          text: 'Ma\'lumot saqlandi!',
          timer: 2000,
          showConfirmButton: false
        });
        this.router.navigate(['/item-view']);
      },
      error: (error) => {
        Swal.fire({
          icon: 'error',
          title: 'Xatolik',
          text: 'Ma\'lumotni saqlab bo‘lmadi. Qayta urinib ko‘ring.',
          confirmButtonText: 'Ok'
        });
        console.error('Xatolik:', error);
      }
    });
  }
}

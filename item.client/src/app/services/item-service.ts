import { inject, Injectable } from '@angular/core';
import { ItemApiService } from "../api/Item-api-service";

@Injectable({
  providedIn: 'root'
})

export class ItemService {
  public itemApiService: ItemApiService = inject(ItemApiService);
}
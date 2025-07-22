import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Item } from '../services/models/item';
import { ItemGetAll } from './model/item-get-all';

@Injectable({
  providedIn: 'root'
})

export class ItemApiService {
  private _apiUrl = `${environment.apiBaseUrl}/Item`;
  private _httpClient = inject(HttpClient);

  public getItems(page: number, pageSize: number, isAscending: boolean, orderBy: string): Observable<ItemGetAll> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize)
      .set('isAscending', isAscending.toString())
      .set('orderBy', orderBy);

    return this._httpClient.get<ItemGetAll>(this._apiUrl, { params });
  }

  public deleteItem(itemId: number): Observable<Item> {
    const params = { id: itemId };
    return this._httpClient.delete<Item>(this._apiUrl, { params });
  }

  public updateItem(itemData: any): Observable<Item> {
    return this._httpClient.put<Item>(this._apiUrl, itemData);
  }

  public addItem(itemData: any): Observable<Item> {
    return this._httpClient.post<Item>(this._apiUrl, itemData);
  }
}

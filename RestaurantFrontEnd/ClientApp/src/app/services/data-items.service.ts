import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from "rxjs";
import { map, catchError } from "rxjs/operators"
import { Item } from '../models/item';
import { GlobalConstants } from './global-constants';
const uuidv1 = require('uuid/v1');
@Injectable({
  providedIn: 'root'
})
export class DataItemsService {
 
  constructor(private httpClient: HttpClient) { }
  getItems(): Observable<Item[]> {
    return this.httpClient.get<Item[]>(GlobalConstants.API +'Item');
  }
  getItemsByCategory(categoryId: string): Observable<Item[]> {
    return this.httpClient.get<Item[]>(GlobalConstants.API + 'Item/Category/' + categoryId);
  }

  createItem(item:Item): Observable<Item> {
    var id = uuidv1();
    //let item = new Item();
    item.Id = id;

    console.log("Creating Item with unit:" + JSON.stringify(item.Unit));
    item.Unit['items'] = null;
    item.Category['items'] = null;

    let result = this.httpClient.post<Item>(GlobalConstants.API +'Item', item);
    return result;

  }
  updateItem(item: Item): Observable<Item> {
    console.log("Updating Item with unit:" + JSON.stringify(item.Unit));
    item.Unit['items'] = null;
    item.Category['items'] = null;

    let result = this.httpClient.put<Item>(GlobalConstants.API +'Item/' + item.Id, item);
    console.log("Updating Item");
    return result;

  }
  deleteItem(itemId) {
    let result = this.httpClient.delete(GlobalConstants.API +'Item/' + itemId);
    console.log("Deleting Item:" + itemId);
    return result;

  }
}

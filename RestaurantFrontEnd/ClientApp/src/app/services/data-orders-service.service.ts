import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from "rxjs";
import { map, catchError } from "rxjs/operators"
import { Order } from '../models/order';
import { GlobalConstants } from './global-constants';
import { OrderedItem } from '../models/ordered-item';
import { NgDate } from '../utility-classes/ng-date';
import { ScreenModel } from '../models/screen-model';
const uuidv1 = require('uuid/v1');
@Injectable({
  providedIn: 'root'
})
export class DataOrdersService {

  constructor(private httpClient: HttpClient) { }
  getOrders(): Observable<Order[]> {
    return this.httpClient.get<Order[]>(GlobalConstants.API + 'Order');
  }


  getOrder(id:string): Observable<Order> {
    return this.httpClient.get<Order>(GlobalConstants.API + 'Order/'+id);
  }

  createOrder(order: Order): Observable<Order> {
    var id = uuidv1();
    //let order = new Order();
    order.Id = id;
    console.log("Creating Order :" + JSON.stringify(order));
    let result = this.httpClient.post<Order>(GlobalConstants.API + 'Order', order);
    return result;

  }

  getOrderedItems(id: string): Observable<OrderedItem[]> {
    return this.httpClient.get<OrderedItem[]>(GlobalConstants.API + 'Order/OrderedItems/' +id);
  }

  updateOrder(order: Order): Observable<Order> {
    console.log("Updating Order :" + JSON.stringify(order));
    let result = this.httpClient.put<Order>(GlobalConstants.API + 'Order/' + order.Id, order);
    console.log("Updating Order");
    return result;

  }
  deleteOrder(orderId:string) {
    let result = this.httpClient.delete(GlobalConstants.API + 'Order/' + orderId);
    console.log("Deleting Order:" + orderId);
    return result;
  }
  deleteOrderedItem(itemId:string) {
    let result = this.httpClient.delete(GlobalConstants.API + 'Order/OrderedItems/' + itemId);
    console.log("Deleting OrderedItem:" +itemId);
    return result;
  }
  addOrderedItem(item: OrderedItem) {
    var id = uuidv1();
    item.Id = id;
    let result = this.httpClient.post(GlobalConstants.API + 'Order/OrderedItems/' + item.Order.Id, item);
    console.log("Adding OrderedItem:" + JSON.stringify(item));
    return result;
  }
  modifyOrderedItem(item: OrderedItem) {
    let result = this.httpClient.put(GlobalConstants.API + 'Order/OrderedItems/' + item.Id, item);
    console.log("Modifying OrderedItem:" + JSON.stringify(item));
    return result;
  }
  getKitchenScreenData(date: Date): Observable<ScreenModel> 
  {
    var ngDate: NgDate = new NgDate(date);
    let result = this.httpClient.get<ScreenModel>(GlobalConstants.API + 'Kitchen/ScreenData/2019-01-01' /*+ ngDate.dateInput*/);
    return result;
    
  }
}

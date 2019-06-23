import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from "rxjs";
import { map, catchError } from "rxjs/operators"
import { Table } from '../models/table';


import { SelectableTable } from '../models/selectable-table';
import { GlobalConstants } from './global-constants';
const uuidv1 = require('uuid/v1');
@Injectable({
  providedIn: 'root'
})
export class DataTablesService {
 
  constructor(private httpClient: HttpClient) { }
  getTables(): Observable<SelectableTable[]> {
    return this.httpClient.get<SelectableTable[]>(GlobalConstants.API +'Table');
  }
  createTable(table: SelectableTable): Observable<SelectableTable> {
    var id = uuidv1();
    table.Id = id;


    let result = this.httpClient.post<SelectableTable>(GlobalConstants.API +'Table', table);
    console.log("Creating Table");
    return result;

  }
  updateTable(table: SelectableTable): Observable<SelectableTable> {
   
    let result = this.httpClient.put<SelectableTable>(GlobalConstants.API +'Table/' + table.Id, table);
    console.log("Updating Table");
    return result;

  }
  deleteTable(tableId) {
    let result = this.httpClient.delete(GlobalConstants.API +'Table/' + tableId);
    console.log("Deleting Table:" + tableId);
    return result;

  }


  updateWaiter(userId: string, tables: SelectableTable[]): Observable<SelectableTable[]> {
    console.log("data-tables.service", userId);
    let result = this.httpClient.post<SelectableTable[]>(GlobalConstants.API + 'WaiterTable/' + userId, tables);
    console.log("Updating Waiter");
    return result;

  }
}

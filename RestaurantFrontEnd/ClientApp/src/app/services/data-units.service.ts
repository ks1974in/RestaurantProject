import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from "rxjs";
import { map, catchError } from "rxjs/operators"
import { Unit } from '../models/unit';
import { GlobalConstants } from './global-constants';

const uuidv1 = require('uuid/v1');
@Injectable({
  providedIn: 'root'
})
export class DataUnitsService {
  
  constructor(private httpClient: HttpClient) { }
  getUnits(): Observable<Unit[]> {
    return this.httpClient.get<Unit[]>(GlobalConstants.API + 'Unit');
  }
  createUnit(unit:Unit): Observable<Unit> {
    var id = uuidv1();
    unit.Id = id;


    let result = this.httpClient.post<Unit>(GlobalConstants.API +'Unit', unit);
    console.log("Creating Unit");
    return result;

  }
  updateUnit(unit: Unit): Observable<Unit> {
   
    let result = this.httpClient.put<Unit>(GlobalConstants.API +'Unit/' + unit.Id, unit);
    console.log("Updating Unit");
    return result;

  }
  deleteUnit(unitId) {
    let result = this.httpClient.delete(GlobalConstants.API +'Unit/' + unitId);
    console.log("Deleting Unit:" + unitId);
    return result;

  }
}

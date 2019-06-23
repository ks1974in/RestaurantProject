import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from "rxjs";
import { map, catchError } from "rxjs/operators"
import { Role } from '../models/role';
import { GlobalConstants } from './global-constants';
const uuidv1 = require('uuid/v1');
@Injectable({
  providedIn: 'root'
})
export class DataRolesService {
 
  constructor(private httpClient: HttpClient) { }
  getRoles(): Observable<Role[]> {
    return this.httpClient.get<Role[]>(GlobalConstants.API +'Role');
  }


  getRoleByName(name: string): Observable<Role> {
    return this.httpClient.get<Role>(GlobalConstants.API + 'Role/Name/' + name);
  }

  createRole(role: Role): Observable<Role> {
    var id = uuidv1();
   
    role.Id = id;


    let result = this.httpClient.post<Role>(GlobalConstants.API +'Role', role);
    console.log("Creating Role:" + JSON.stringify(role));
    return result;

  }
  updateRole(role: Role): Observable<Role> {
   
    let result = this.httpClient.put<Role>(GlobalConstants.API +'Role/' + role.Id, role);
    console.log("Updating Role");
    return result;

  }
  deleteRole(roleId) {
    let result = this.httpClient.delete(GlobalConstants.API +'Role/' + roleId);
    console.log("Deleting Role:" + roleId);
    return result;

  }
}

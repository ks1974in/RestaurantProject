import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from "rxjs";
import { map, catchError } from "rxjs/operators"
import { User } from '../models/user';
import { GlobalConstants } from './global-constants';
import { Waiter } from '../models/waiter';
const uuidv1 = require('uuid/v1');
@Injectable({
  providedIn: 'root'
})
export class DataUsersService {
 
  constructor(private httpClient: HttpClient) { }
  getUsers(): Observable<User[]> {
    return this.httpClient.get<User[]>(GlobalConstants.API+'User');
  }

  getUser(id:string): Observable<User> {
    return this.httpClient.get<User>(GlobalConstants.API + 'User/'+id);
  }

  getUsersByRole(name:string): Observable<User[]> {
    return this.httpClient.get<User[]>(GlobalConstants.API + 'User/Role/Name/'+name);
  }


  getWaitersByTableNumber(number: string): Observable<Waiter[]> {
    return this.httpClient.get<Waiter[]>(GlobalConstants.API + 'User/Waiter/Table/Number/' + number);
  }


  createUser(user: User): Observable<User> {
    var id = uuidv1();
    //let user = new User();
    user.Id = id;


    let result = this.httpClient.post<User>(GlobalConstants.API +'User', user);
    console.log("Creating User:" + JSON.stringify(user));
    return result;

  }

  


  updateUser(user: User): Observable<User> {
  
    let result = this.httpClient.put<User>(GlobalConstants.API +'User/' + user.Id, user);
    console.log("Updating User");
    return result;

  }
  deleteUser(userId) {
    let result = this.httpClient.delete(GlobalConstants.API +'User/' + userId);
    console.log("Deleting User:" + userId);
    return result;

  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from "rxjs";
import { map, catchError } from "rxjs/operators"
import { Category } from '../models/category';
import { GlobalConstants } from './global-constants';
const uuidv1 = require('uuid/v1');
  @Injectable({
  providedIn: 'root'
})
export class DataCategoriesService {
    API_SERVER="http://localhost:8080/";
    constructor(private httpClient: HttpClient) { }
    getCategories(): Observable<Category[]>
    {
      return this.httpClient.get<Category[]>(GlobalConstants.API +'Category');
    }
    createCategory(category:Category): Observable<Category>
    {
      var id = uuidv1();
      category.Id = id;
      
      
      let result = this.httpClient.post<Category>(GlobalConstants.API +'Category', category);
      console.log("Creating Category");
      return result;
      
    }
   updateCategory(category:Category): Observable<Category> {
     
     let result = this.httpClient.put<Category>(GlobalConstants.API +'Category/' + category.Id, category);
      console.log("Updating Category");
      return result;

    }
    deleteCategory(categoryId)
    {
      let result = this.httpClient.delete(GlobalConstants.API +'Category/'+ categoryId);
      console.log("Deleting Category:"+categoryId);
      return result;

    }
}

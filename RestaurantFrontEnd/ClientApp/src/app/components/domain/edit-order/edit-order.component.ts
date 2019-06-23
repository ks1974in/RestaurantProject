import { Component, OnInit, OnDestroy, Renderer } from '@angular/core';
import { FormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { Event, Route, Router } from "@angular/router";
import { Observable, pipe } from 'rxjs';
import { map, finalize } from 'rxjs/operators';
import { ActivatedRoute } from "@angular/router";


import { DataItemsService } from "../../../services/data-items.service";
import { DataCategoriesService } from "../../../services/data-categories.service";
import { DataUnitsService } from "../../../services/data-units.service";

import { Category } from '../../../models/category';
import { Unit } from '../../../models/unit';
import { Item } from '../../../models/item';
import { GlobalConstants } from '../../../services/global-constants';
import { FormState } from '../../../utility-classes/form-state';
import { Order } from '../../../models/order';
import { OrderedItem } from '../../../models/ordered-item';
import { DataOrdersService } from '../../../services/data-orders-service.service';


@Component({
  selector: 'app-edit-order',
  templateUrl: './edit-order.component.html',
  styleUrls: ['./edit-order.component.css']
})
export class EditOrderComponent implements OnInit {
  formState: FormState = new FormState();
  currentOrder: Order;
  categories: Category[] = [];
  units: Unit[] = [];
  items: Item[] = [];
  columnCount: number = 5;
  rowCount: number = 0;
  listener: any;
  itemArray: Item[][] = [];
  categoryArray: Category[][]=[];
  currentItem: OrderedItem;
  orderedItems: OrderedItem[];
  

  constructor(private route: ActivatedRoute, private renderer: Renderer, private router: Router, private formBuilder: FormBuilder, private data: DataItemsService, private dataOrders: DataOrdersService, private dataCategories: DataCategoriesService, private dataUnits: DataUnitsService) 
  {
  this.route.params.subscribe(
    params => {
      console.log(params);
      this.getOrder(params['orderId']);
    }
  );
  }
  getOrder(id) {

    this.dataOrders.getOrder(id)
      .pipe(finalize(() => { console.log('Order Fetched'); })).subscribe((order: Order) => {
        this.currentOrder = order;
        
      });
  }
  onBack() {
    this.router.navigate(['viewOrders'], { skipLocationChange: true });
  }

  ngOnInit() {
    this.refresh();
  }
  ngOnDestroy(): void {
   this.listener();
  }
  

  ngAfterViewInit(): void {

    this.listener = this.renderer.listen('document', 'click', (evt) => {
      
      if (evt.target.id == 'category') {
        this.onCategoryClick(evt.target.getAttribute("categoryId"));
      }

      else if (evt.target.id == 'item') {
        this.onItemClick(evt.target.getAttribute("itemId"));
      }
      
   
    });

  }

  onCategoryClick(id) {
    let that = this;  
    console.log('Category selected:' + id);
    this.data.getItemsByCategory(id)
      .pipe(finalize(() => {

      }))
      .subscribe((items: Item[]) => {
        console.log("Items fetched");

        console.log(items.length);
        //console.log(JSON.stringify(items));
        var columnCount: number = 5;
        var rowCount: number = (items.length / columnCount);
        var row: number = 0
        var col: number = 0;
        that.itemArray = new Array();
        for (row = 0; row < rowCount; row++) {
          that.itemArray[row] = [];
          for (col = 0; col < columnCount; col++) {
            var i: number = row * columnCount + col;
            console.log(row, col, i);
            that.itemArray[row][col] = items[i];
          }
        }
       
        console.log(JSON.stringify(this.itemArray));
        this.items = items;
      });

  }
  computeSubtotal() {
    this.currentItem.Subtotal = this.currentItem.Price * this.currentItem.Quantity;
  }
  onItemClick(id)
  {
    var item: Item = this.items.find(x => x.Id == id);
    this.currentItem = new OrderedItem({ Item: item, Order: this.currentOrder } as OrderedItem);
  }
  onAdd()
  {
    this.dataOrders.addOrderedItem(this.currentItem)
      .pipe(finalize(() => {
        this.currentItem = new OrderedItem();
      }))
      .subscribe((categories: Category[]) => {
        console.log("ordered item added");
      });
  }

  onCancel()
  {
    this.currentItem = new OrderedItem();
  }

  refresh() {
    let that = this;
    try {

      this.dataCategories.getCategories()
        .pipe(finalize(() => {

        }))
        .subscribe((categories: Category[]) => {
          console.log("categories fetched");

          console.log(categories.length);
          
          var columnCount: number = 5;
          var rowCount: number = (categories.length / columnCount);
          var row: number = 0
          var col: number = 0;
         
          that.categoryArray = [];
          for (row = 0; row < rowCount; row++) {
            that.categoryArray[row] = [];
            for (col = 0; col < columnCount; col++) {
              var i: number = row * columnCount + col;
              console.log(row, col, i);
              if (i >= categories.length)
              {
                //console.log('End of array');
                that.categoryArray[row][col] = new Category();
              }
              else
              {
                that.categoryArray[row][col] = categories[i];
                console.log(i, JSON.stringify(that.categoryArray[row][col]));
              }

              
              
            }
          }
         
          console.log(JSON.stringify(this.categoryArray));
          this.categories = categories;
        });


      
    }
    catch (e) { console.log(e); }
  }



}

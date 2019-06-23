import { Component, OnInit, Renderer } from '@angular/core';
import { Category } from '../../../models/category';
import { Router } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { DataItemsService } from '../../../services/data-items.service';
import { DataCategoriesService } from '../../../services/data-categories.service';
import { DataUnitsService } from '../../../services/data-units.service';
import { FormState } from '../../../utility-classes/form-state';
import { finalize } from 'rxjs/operators';
import { GlobalConstants } from '../../../services/global-constants';
import { Subject } from 'rxjs';
import { Order } from '../../../models/order';

import { DataOrdersService } from '../../../services/data-orders-service.service';
import { Table } from '../../../models/table';
import { Waiter } from '../../../models/waiter';
import { DataTablesService } from '../../../services/data-tables.service';
import { DataUsersService } from '../../../services/data-users.service';
import { NgDate } from '../../../utility-classes/ng-date';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<Order> = new Subject();
  currentOrder: Order;
  tables: Table[]=[];
  waiters: Waiter[]=[];
  NgDate: NgDate;
  listener: any;
  formState: FormState = new FormState();
  tempCategory: Category;
  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.Id === c2.Id : c1 === c2;
  }
  constructor(private renderer: Renderer, private router: Router, private formBuilder: FormBuilder, private data: DataOrdersService, private dataTables: DataTablesService, private dataWaiters: DataUsersService) { }

  

  ngOnInit() {

   
    this.refresh();

  }


  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    this.listener();
  }

  ngAfterViewInit(): void {


    this.listener = this.renderer.listen('document', 'click', (evt) => {

      if (evt.target.id == 'delete') {
        this.onDelete(evt.target.getAttribute("itemId"));
      }
      else if (evt.target.id == 'edit') {
        this.onEdit(evt.target.getAttribute("itemId"));
      }

    });

  }
  onView(id) {
    if (this.formState.inAdd || this.formState.inEdit) {
      if (!confirm("Are you sure you wish to cancel this operation?")) return;
    }

    /*
    this.currentItem = this.items.find(function (element) { return element.Id == id; })
    */
    this.formState.setFormToViewState();
  }

  onCancel(form) { } 

  onDelete(id) {

    if (!confirm('Do you wish to delete this currentItem?')) return;
    console.log('onDelete:' + id);
    this.data.deleteOrderedItem(id)
      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/Order', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/Order/OrderedItems' + this.currentOrder.Id]));

        this.formState.setFormToViewState();


      }))

      .subscribe(
        (currentItem) => { console.log("currentItem deleted:" + id); },
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })
  }
  createOrder() {
    console.log('createOrder');
    this.data.createOrder(this.currentOrder)

      .pipe(finalize(() => {
        this.formState.setFormToViewState();
       
        this.refresh();
       
      }))

      .subscribe(
        (order) => {
          console.log("order created:" + JSON.stringify(order));
          this.currentOrder = order;
        
        },
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })

  }

  updateOrder() {
    this.data.updateOrder(this.currentOrder)

      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/createOrders', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/createOrders']));


        this.formState.setFormToViewState();
      }))

      .subscribe(
        (order) => {
          console.log("order updated:" + JSON.stringify(order));
          this.currentOrder = order;
        
        }
        ,
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })

  }

  onSubmit(form) {
    console.log('onSubmit');
    this.formState.submitted = true;
    if (form.invalid) {
     // return false;
    }
    if (this.formState.inAdd) {
      this.createOrder();
    }
    else if (this.formState.inEdit) {
      this.updateOrder();
    }
  }
  randomIntFromInterval(min, max) // min and max included
{
  return Math.floor(Math.random() * (max - min + 1) + min);
}

  onCreate() {
    this.currentOrder = new Order();
    this.NgDate = new NgDate();
    this.currentOrder.Date = new Date();
    console.log(this.currentOrder.Date);
    this.currentOrder.Amount = 0;
    this.currentOrder.Billed = false;
    this.currentOrder.Completed = false;
    this.currentOrder.Discount = 0;
    this.currentOrder.Remarks = "None";
    this.currentOrder.Subtotal = 0;
    this.currentOrder.Taxes = 0;
    this.currentOrder.Total = 0;
    this.currentOrder.OrderNumber = this.randomIntFromInterval(1, 2000) + '';
    
    this.formState.setFormToAddState();
    this.refresh();
  }
  onEdit(itemId) {
     this.formState.setFormToEditState();
  }



  getWaiters() {
    
    this.dataWaiters.getWaitersByTableNumber(this.currentOrder.Table.Number)
      .pipe(finalize(() => {
        if (this.waiters.length > 0) {
          console.log(this.waiters.length + "Waiters fetched");
          this.currentOrder.Waiter = this.waiters[0];
        }
        else {
          console.log("No Waiters found");
        }
       
      }))
      .subscribe((waiters: Waiter[]) => {
       
        this.waiters = waiters;
        JSON.stringify(waiters);
        
      });
  }

  getWaiterName() {
    return this.currentOrder.Waiter.Name;
  }
  refresh() {
    this.dataTables.getTables()
      .pipe(finalize(() => {
        console.log(this.tables.length + ' tables fetched');
        this.getWaiters();
        

      })).subscribe((tables: Table[]) => {
        this.tables = tables;
        if (tables.length > 0) this.currentOrder.Table = tables[0];
        console.log(JSON.stringify(this.currentOrder.Table));
      });
  }

  
}

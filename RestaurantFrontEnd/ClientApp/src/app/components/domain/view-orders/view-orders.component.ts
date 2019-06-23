import { Component, OnInit, OnDestroy, Renderer } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';


import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { Event, Route, Router } from "@angular/router";
import { Observable } from 'rxjs';
import { map, finalize } from 'rxjs/operators';
import { pipe } from 'rxjs';
import { GlobalConstants } from '../../../services/global-constants';
import { Order } from '../../../models/order';
import { DataOrdersService } from '../../../services/data-orders-service.service';
import { FormState } from '../../../utility-classes/form-state';


@Component({
  selector: 'app-view-orders',
  templateUrl: './view-orders.component.html',
  styleUrls: ['./view-orders.component.css']
})
export class ViewOrdersComponent implements OnInit, OnDestroy {
  [x: string]: any;
  //Data Table
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<Order> = new Subject();
  orders: Order[];
  listener: any;
  formState: FormState = new FormState();
  constructor(private renderer: Renderer, private router: Router, private data: DataOrdersService) { }




  buildOptions() {

    this.dtOptions = {
      ajax: {
        url: GlobalConstants.API + 'Order', dataSrc: ""
      },
      pagingType: 'full_numbers',
      pageLength: 3,
      destroy: true,
      columns: [{
        title: 'Number',
        data: 'OrderNumber',
        className: 'dt-center'
      },
      {
        title: 'Date',
        data: 'Date',
        className: 'dt-center'
        },
        {
          title: 'Table',
          data: 'Table.Number',
          className: 'dt-center'
        },
        {
          title: 'Waiter',
          data: 'Waiter.Name',
          className: 'dt-center'
        },
        {
          title: 'Total',
          data: 'Total',
          className: 'dt-center'
        },
      {
        title: 'View',
        render: function (data: any, type: any, full: any) {

          return "<img  id='view' orderId='" + full.Id + "' class='btn view'/ src='assets/icons/view.png' >";
        },
        className: 'dt-center'
      }
        , {
        title: 'Edit',
        render: function (data: any, type: any, full: any) {

          return "<img  id='edit' orderId='" + full.Id + "' class='btn edit' src='assets/icons/edit.png' />";
        },
        className: 'dt-center'
      }
        , {
        title: 'Delete',
        render: function (data: any, type: any, full: any) {
          return "<img  id='delete' orderId='" + full.Id + "' class='btn delete' src='assets/icons/delete.png'/>";
        },
        className: 'dt-center'
      }],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
       
        $('td', row).unbind('click');
        $('td', row).bind('click', () => {
      
        });
        return row;
      }
      ,

    }
  }



  ngOnInit() {
    this.buildOptions();

    this.refresh();


  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    this.listener();
  }


  ngAfterViewInit(): void {

    this.listener = this.renderer.listen('document', 'click', (evt) => {

      if (evt.target.id == 'delete') {
        this.onDelete(evt.target.getAttribute("orderId"));
      }
      else if (evt.target.id == 'edit') {
        this.onEdit(evt.target.getAttribute("orderId"));
      }
      else if (evt.target.id == 'view') {
        this.onView(evt.target.getAttribute("orderId"));
      }
    });

  }

  onView(id) {
    console.log('onView()');
    this.router.navigate(['viewOrder', id], { skipLocationChange: true });
  }

  onEdit(id)
  {
    console.log('onEdit()');
    this.router.navigate(['editOrder', id], { skipLocationChange: true });
  }
  onDelete(id) {
    console.log('onDelete()');
    if (!confirm('Do you wish to delete this item?')) return;
    console.log('onDelete:' + id);
    this.data.deleteOrder(id)
      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/viewOrders', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/viewOrders']));
      }))
      .subscribe((orders: Order[]) => { this.orders = orders; });
  }




  refresh() {
    try {
      this.data.getOrders()
        .pipe(finalize(() => {
          console.log(this.orders.length + ' orders fetched');
         // console.log(JSON.stringify(this.orders));
          this.dtTrigger.next();
          var myTable = $('#tableId').DataTable();
          myTable.clear().rows.add(this.orders).draw();
        })).subscribe((orders: Order[]) => { this.orders = orders; });

    }
    catch (e) { console.log(e); }
  }


}

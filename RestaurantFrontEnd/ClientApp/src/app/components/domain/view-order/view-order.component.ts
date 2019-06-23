import { Component, OnInit, Renderer } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from "@angular/router";
import { DataOrdersService } from '../../../services/data-orders-service.service';
import { Order } from '../../../models/order';
import { finalize } from 'rxjs/operators';
import { NgDate } from '../../../utility-classes/ng-date';
import { FormState } from '../../../utility-classes/form-state';
import { OrderedItem } from '../../../models/ordered-item';
import { Subject } from 'rxjs';
import { GlobalConstants } from '../../../services/global-constants';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-view-order',
  templateUrl: './view-order.component.html',
  styleUrls: ['./view-order.component.css']
})
export class ViewOrderComponent implements OnInit {

  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<OrderedItem> = new Subject();
  listener: any;
  NgDate: NgDate = new NgDate();
  currentOrder: Order;
  formState: FormState = new FormState();
  items: OrderedItem[];
  constructor(private renderer: Renderer,private route: ActivatedRoute, private router: Router, private data: DataOrdersService)
  {
    this.route.params.subscribe(
      params =>
      {
        console.log(params);
        this.getOrder(params['orderId']);
      }
    );
  }

  ngOnInit() {
  
  }
  getOrder(id) {

    this.data.getOrder(id)
      .pipe(finalize(() => {

        

        var myTable = $('#tableId').DataTable();
        myTable.ajax.reload();
        this.dtTrigger.next();
        //myTable.clear().rows.add(this.currentOrder.Items).draw();
        console.log('Order Fetched');
      })).subscribe((order: Order) => {
        this.currentOrder = order;
        this.NgDate = new NgDate(this.currentOrder.Date);
       // console.log(JSON.stringify(this.currentOrder));
        this.buildOptions();     
        
        
      })
  }
  back() {
    this.router.navigate(['viewOrders'], { skipLocationChange: true });
  }




  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    this.listener();
  }

  buildOptions() {

    this.dtOptions = {
      ajax: {
        url: GlobalConstants.API + 'Order/OrderedItems/' + this.currentOrder.Id, dataSrc: ""
      },
      pagingType: 'full_numbers',
      pageLength: 5,
      destroy: true,
      
      columns: [{
        title: 'Name',
        data: 'Item.Name',
        className: 'dt-center'
      },
      {
        title: 'Price',
        data: 'Price',
        className: 'dt-center'
        },
        {
          title: 'Quantity',
          data: 'Quantity',
          className: 'dt-center'
        },
        {
          title: 'Subtotal',
          data: 'Subtotal',
          className: 'dt-center'
        },
      {
        title: 'View',
        render: function (data: any, type: any, full: any) {

          return "<img  id='view' orderedItemId='" + full.Id + "' class='btn view'/ src='assets/icons/view.png' >";
        },
        className: 'dt-center'
      },
       {
        title: 'Delete',
        render: function (data: any, type: any, full: any) {
          return "<img  id='delete' orderedItemId='" + full.Id + "' class='btn delete' src='assets/icons/delete.png'/>";
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
    console.log('buildOptions finished');
  }
  onEdit() { }
  onView(id) { }
  onDelete(id) { }
  ngAfterViewInit(): void {

    this.listener = this.renderer.listen('document', 'click', (evt) => {

      if (evt.target.id == 'delete') {
        this.onDelete(evt.target.getAttribute("orderedItemId"));
      }
      else if (evt.target.id == 'edit') {
        this.onView(evt.target.getAttribute("orderedItemId"));
      }
   
    });

  }

}

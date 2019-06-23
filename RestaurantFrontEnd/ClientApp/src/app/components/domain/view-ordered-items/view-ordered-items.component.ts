import { Component, OnInit, OnDestroy, Renderer } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { Event, Route, Router, ActivatedRoute } from "@angular/router";
import { Observable } from 'rxjs';
import { map, finalize } from 'rxjs/operators';
import { pipe } from 'rxjs';
import { GlobalConstants } from '../../../services/global-constants';
import { HttpClient } from '@angular/common/http';

import { Order } from '../../../models/order';
import { FormState } from '../../../utility-classes/form-state';
import { OrderedItem } from '../../../models/ordered-item';
import { DataOrdersService } from '../../../services/data-orders-service.service';
import { NgDate } from '../../../utility-classes/ng-date';


@Component({
  selector: 'app-view-ordered-items',
  templateUrl: './view-ordered-items.component.html',
  styleUrls: ['./view-ordered-items.component.css']
})
export class ViewOrderedItemsComponent implements OnInit, OnDestroy {
  [x: string]: any;
  //Data Table
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<Order> = new Subject();
  currentOrder: Order;
  NgDate: NgDate = new NgDate();
  orderedItems: OrderedItem[];
  listener: any;
  formState: FormState = new FormState();
  constructor(private renderer: Renderer, private route: ActivatedRoute, private router: Router, private data: DataOrdersService)
  {
    this.route.params.subscribe(
      params => {
        console.log(params);
        this.getOrder(params['orderId']);
      }
    );
  }

  


  buildOptions() {

    this.dtOptions = {
      ajax: {
        url: GlobalConstants.API + 'Order/OrderedItems/'+this.currentOrder.Id, dataSrc: ""
      },
      pagingType: 'full_numbers',
      pageLength: 3,
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
          title: 'Category',
          data: 'Item.Category.Name',
          className: 'dt-center'
        },
        {
          title: 'Category Code',
          data: 'Item.Category.Code',
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
  }



  ngOnInit() {
   
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    this.listener();
  }


  ngAfterViewInit(): void {

    this.listener = this.renderer.listen('document', 'click', (evt) => {

      if (evt.target.id == 'delete') {
        this.onDelete(evt.target.getAttribute("orderedItemId"));
      }
      /*      else if (evt.target.id == 'edit') {
        this.onEdit(evt.target.getAttribute("orderedItemId"));
      }
      else if (evt.target.id == 'view') {
        this.onView(evt.target.getAttribute("orderedItemId"));
      }*/
    });

  }


  onBack() {
    this.router.navigate(['viewOrders'], { skipLocationChange: true });
  }

  onView() {
    this.router.navigate(['viewOrder', this.currentOrder.Id], { skipLocationChange: true });
  }

  onEdit() {
    this.router.navigate(['editOrder', this.currentOrder.Id], { skipLocationChange: true });
  }
  onDelete(id) {

    if (!confirm('Do you wish to delete this item?')) return;
    console.log('onDelete:' + id);
    this.data.deleteOrderedItem(id)
      .pipe(finalize(() => {

        
        this.router.navigateByUrl('/', { skipLocationChange: true }).then(() =>
          this.router.navigate(['viewOrder',this.currentOrder.Id]));
      }))
      .subscribe((orderedItems: Order[]) => { this.items = orderedItems; });
  }



  getOrder(id) {
    try {
      this.data.getOrder(id)
        .pipe(finalize(() => {
          this.NgDate = new NgDate(this.currentOrder.Date);

          this.data.getOrderedItems(id)
            .pipe(finalize(() => {



              console.log(this.orderedItems.length + ' orderedItems fetched');
              console.log(JSON.stringify(this.orderedItems));
              this.dtTrigger.next();
              var myTable = $('#tableId').DataTable();
              myTable.clear().rows.add(this.orderedItems).draw();

            })).subscribe((orderedItems: OrderedItem[]) => { this.orderedItems=orderedItems});

        
        })).subscribe((order: Order) => { this.currentOrder = order; this.buildOptions();});

    }
    catch (e) { console.log(e); }
  }

  

}

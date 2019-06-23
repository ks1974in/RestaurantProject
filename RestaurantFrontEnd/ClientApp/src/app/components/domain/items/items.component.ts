import { Component, OnInit, OnDestroy, Renderer } from '@angular/core';
import { FormsModule,FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { Event, Route, Router } from "@angular/router";
import { Observable, pipe } from 'rxjs';
import { map, finalize } from 'rxjs/operators';
import { FormState } from '../../../utility-classes/form-state';


import { DataItemsService } from "../../../services/data-items.service";
import { DataCategoriesService } from "../../../services/data-categories.service";
import { DataUnitsService } from "../../../services/data-units.service";

import { Category } from '../../../models/category';
import { Unit } from '../../../models/unit';
import { Item } from '../../../models/item';
import { GlobalConstants } from '../../../services/global-constants';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent implements OnInit, OnDestroy {
  [x: string]: any;
  //Data Table
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<Item> = new Subject();


  listener: any;
  categories: Category[] = [];
  units: Unit[] = [];
  //Models
  items: Item[] = [];
  currentItem: Item;


  formState: FormState = new FormState();

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.Id === c2.Id : c1 === c2;
  }



  constructor(private renderer: Renderer, private router: Router, private formBuilder: FormBuilder, private data: DataItemsService, private dataCategories: DataCategoriesService, private dataUnits: DataUnitsService) { }




  buildOptions() {

    this.dtOptions = {
      ajax: {
        url: GlobalConstants.API +'Item', dataSrc: "" },
      pagingType: 'full_numbers',
      pageLength: 3,
      destroy: true,
      columns: [{
        title: 'Name',
        data: 'Name',
        className: 'dt-center'
      },
      {
        title: 'Code',
        data: 'Code',
        className: 'dt-center'
        },
        {
          title: 'Unit',
          data: 'Unit.Name',
          className: 'dt-center'
        },
        {
          title: 'Price',
          data: 'Price',
          className: 'dt-center'
        },
        {
          title: 'Category',
          data: 'Category.Name',
          className: 'dt-center'
        },
        {
          title: 'Category Code',
          data: 'Category.Code',
          className: 'dt-center'
        },
      {
        title: 'View',
        render: function (data: any, type: any, full: any) {

          return "<img  id='view' itemId='" + full.Id + "' class='btn view'/ src='assets/icons/view.png' >";
        },
        className: 'dt-center'
      }
        , {
        title: 'Edit',
        render: function (data: any, type: any, full: any) {

          return "<img  id='edit' itemId='" + full.Id + "' class='btn edit' src='assets/icons/edit.png' [disabled]='editButtonDisabled'/>";
        },
        className: 'dt-center'
      }
        , {
        title: 'Delete',
        render: function (data: any, type: any, full: any) {
          return "<img  id='delete' itemId='" + full.Id + "' class='btn delete' src='assets/icons/delete.png'/>";
        },
        className: 'dt-center'
      }],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        
        $('td', row).unbind('click');
        $('td', row).bind('click', () => {
          this.onView((<Item>data).Id);
        });
        return row;
      }
      ,

    }
  }



  ngOnInit() {

    this.currentItem = new Item();
    this.buildOptions();
    this.dataCategories.getCategories()
      .pipe(finalize(() => {
        console.log(this.categories.length + ' caetegories fetched');
        this.dataUnits.getUnits()
          .pipe(finalize(() => {
            console.log(this.units.length + ' units fetched');
           
            this.refresh();
          })).subscribe((units: Unit[]) => { this.units = units; });
    

      })).subscribe((categories: Category[]) => { this.categories = categories; });

    
    
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
      /*
    else if (evt.target.Id == 'view') {
      this.viewItem(evt.target.getAttribute("itemId"));
    }*/
    });

  }
  refresh() {
    try {
      this.data.getItems()
        .pipe(finalize(() => {
          this.dtTrigger.next();
          this.currentItem = new Item();
          if (this.categories.length > 0)
            this.currentItem.Category = this.categories[0];
          if (this.units.length > 0)
            this.currentItem.Unit = this.units[0];
          this.formState.setFormToViewState();
        }))
        .subscribe((items: Item[]) => {
          console.log("Items fetched");
          this.items = items;
          console.log(items.length);
          console.log(JSON.stringify(items));
          var myTable = $('#tableId').DataTable();
          myTable.clear().rows.add(this.items).draw();

        });
    }
    catch (e) { console.log(e); }
  }


  onCreate() {
    this.currentItem = new Item();
    if (this.categories.length > 0)
      this.currentItem.Category = this.categories[0];
    if (this.units.length > 0)
      this.currentItem.Unit = this.units[0];
    this.formState.setFormToAddState();
  }
  onEdit(itemId) {
    var id = itemId;
    this.currentItem = this.items.find(function (element) { return element.Id == id; })
    if (this.currentItem == null) {
      this.currentItem = new Item();
    }
    this.formState.setFormToEditState();
  }




  createItem() {
    
    this.data.createItem(this.currentItem)

      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/items', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/items']));
        this.currentItem = new Item();
        this.formState.setFormToViewState();
      }))

      .subscribe(
        (item) => { console.log("item created:" + JSON.stringify(item)); },
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })

  }

  updateItem() {
   
    this.data.updateItem(this.currentItem)

      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/items', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/items']));


        this.formState.setFormToViewState();
      }))

      .subscribe(
        (item) => { console.log("item created:" + JSON.stringify(item)); },
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })

  }


  onDelete(id) {

    if (!confirm('Do you wish to delete this item?')) return;
    console.log('onDelete:' + id);
    this.data.deleteItem(id)
      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/items', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/items']));

        this.formState.setFormToViewState();


      }))

      .subscribe(
        (item) => { console.log("item deleted:" + id); },
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })
  }



  onSubmit(form) {
    this.formState.submitted = true;
    if (form.invalid) {
      return false;
    }
    if (this.formState.inAdd) {
      this.createItem();
    }
    else if (this.formState.inEdit) {
      this.updateItem();
    }
  }
  onView(id) {
    if (this.formState.inAdd || this.formState.inEdit) {
      if (!confirm("Are you sure you wish to cancel this operation?")) return;
    }

    this.currentItem = this.items.find(function (element) { return element.Id == id; })

    this.formState.setFormToViewState();
  }


  onCancel(form) {
    this.currentItem = new Item();
    this.formState.setFormToViewState();
  }


}

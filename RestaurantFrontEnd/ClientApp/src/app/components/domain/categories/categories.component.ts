import { Component, OnInit, OnDestroy, Renderer, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';


import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { Event, Route, Router } from "@angular/router";
import { Observable } from 'rxjs';
import { map, finalize} from 'rxjs/operators';
import { pipe } from 'rxjs';
import { FormState } from '../../../utility-classes/form-state';

import { DataCategoriesService } from "../../../services/data-categories.service";
import { Category } from '../../../models/category';

import { GlobalConstants } from '../../../services/global-constants';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit, OnDestroy {
    [x: string]: any;
  //Data Table
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<Category> = new Subject();


  //Models
  categories: Category[] = [];
  currentCategory: Category;
  
  listener: any;
  formState: FormState = new FormState();
    
  



  constructor(private cd: ChangeDetectorRef,private renderer: Renderer, private router: Router, private httpClient: HttpClient, private formBuilder: FormBuilder, private data: DataCategoriesService) { }
 
  buildOptions() {

    this.dtOptions = {
      ajax: {
        url: GlobalConstants.API +'Category', dataSrc: "" },
      pagingType: 'full_numbers',
      pageLength: 3,
      destroy: true,
      columns: [{
        title: 'Name',
        data: 'Name',
        className:'dt-center'
      },
        {
          title: 'Code',
          data: 'Code',
          className: 'dt-center'
        },
      {
        title: 'Description',
        data: 'Description',
        className: 'dt-center'
      },
      {
        title: 'View',
        render: function (data: any, type: any, full: any) {

          return "<img  id='view' categoryId='" + full.Id + "' class='btn view'/ src='assets/icons/view.png'>";
        },
        className: 'dt-center'
      }
        , {
        title: 'Edit',
        render: function (data: any, type: any, full: any) {

          return "<img  id='edit' categoryId='" + full.Id + "' class='btn edit' src='assets/icons/edit.png' [disabled]='editButtonDisabled'/>";
          },
          className: 'dt-center'
      }
        , {
        title: 'Delete',
        render: function (data: any, type: any, full: any) {
          return "<img  id='delete' categoryId='" + full.Id + "' class='btn delete' src='assets/icons/delete.png'/>";
          },
          className: 'dt-center'
      }],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
       
        $('td', row).unbind('click');
        $('td', row).bind('click', () => {
            this.onView((<Category>data).Id);
        });
        return row;
      }
      ,

    }
  }

  
  
  ngOnInit() {
    this.currentCategory = new Category();
    this.buildOptions();
  this.refresh();
    
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    this.listener();
  }


  ngAfterViewInit(): void {

    this.listener = this.renderer.listen('document', 'click', (evt) => {
      console.log('Event Fired');
      if (evt.target.id == 'delete') {
        this.onDelete(evt.target.getAttribute("categoryId"));
      }
      else if (evt.target.id == 'edit') {
        this.onEdit(evt.target.getAttribute("categoryId"));
      }
        /*
      else if (evt.target.Id == 'view') {
        this.viewCategory(evt.target.getAttribute("categoryId"));
      }*/
   });
    
  }
  refresh() {
    try {

      
      this.data.getCategories()
        .pipe(finalize(() => {
          this.dtTrigger.next();
          this.currentCategory = new Category();
          this.formState.setFormToViewState();
          this.cd.detectChanges();
        }))
        .subscribe((categories: Category[]) => {
        console.log("Categories fetched");
        this.categories = categories;
          console.log(categories.length);
          console.log(JSON.stringify(categories));
          var myTable = $('#tableId').DataTable();
          myTable.clear().rows.add(this.categories).draw();        
          
      });
    }
    catch (e) { console.log(e); }
  }


  onCreate() {
    this.currentCategory = new Category();
    this.formState.setFormToAddState();
  }
  onEdit(categoryId) {
    console.log('onEdit:' + categoryId);
    var id = categoryId;
    this.currentCategory = this.categories.find(function (element) { return element.Id == id; })
    if (this.currentCategory == null) {
      this.currentCategory = new Category();
    }
    this.formState.setFormToEditState();
  }




  createCategory() {
    this.data.createCategory(this.currentCategory)

      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/categories', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/categories']));
        this.currentCategory = new Category();
        this.formState.setFormToViewState();
      }))

      .subscribe(
        (category) => { console.log("category created:" + JSON.stringify(category)); },
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })

  }
 
  updateCategory() {
    this.data.updateCategory(this.currentCategory)

      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/categories', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/categories']));
        

        this.formState.setFormToViewState();
      }))

      .subscribe(
        (category) => { console.log("category created:" + JSON.stringify(category)); },
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })

  }


  onDelete(id) {
    console.log('onDelete:' + id);
    if (!confirm('Do you wish to delete this category?')) return;
    console.log('onDelete:' + id);
    this.data.deleteCategory(id)
      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/categories', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/categories']));

        this.formState.setFormToViewState();


      }))

      .subscribe(
        (category) => { console.log("category deleted:" + id); },
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
      this.createCategory();
    }
    else if (this.formState.inEdit) {
      this.updateCategory();
    }
}
  onView(id) {
    if (this.formState.inAdd || this.formState.inEdit) {
      if (!confirm("Are you sure you wish to cancel this operation?")) return;
    }
   
    this.currentCategory = this.categories.find(function (element) { return element.Id == id; })

    this.formState.setFormToViewState();
  }


  onCancel(form)
  {
    this.currentCategory = new Category();
    this.formState.setFormToViewState();
  }

 
}

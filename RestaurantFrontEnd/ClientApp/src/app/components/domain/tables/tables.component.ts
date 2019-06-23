import { Component, OnInit, OnDestroy, Renderer, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';


import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';
import { Event, Route, Router } from "@angular/router";
import { Observable } from 'rxjs';
import { map, finalize } from 'rxjs/operators';
import { pipe } from 'rxjs';
import { FormState } from '../../../utility-classes/form-state';

import { DataTablesService } from "../../../services/data-tables.service";
import { SelectableTable } from '../../../models/selectable-table';

import { GlobalConstants } from '../../../services/global-constants';
@Component({
  selector: 'app-tables',
  templateUrl: './tables.component.html',
  styleUrls: ['./tables.component.css']
})
export class TablesComponent implements OnInit, OnDestroy {
  [x: string]: any;
  //Data SelectableTable
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<SelectableTable> = new Subject();


  //Models
  tables: SelectableTable[] = [];
  currentTable: SelectableTable;

  listener: any;
  formState: FormState = new FormState();





  constructor(private cd: ChangeDetectorRef, private renderer: Renderer, private router: Router, private httpClient: HttpClient, private formBuilder: FormBuilder, private data: DataTablesService) { }

  buildOptions() {

    this.dtOptions = {
      ajax: {
        url: GlobalConstants.API +'Table', dataSrc: "" },
      pagingType: 'full_numbers',
      pageLength: 3,
      destroy: true,
      columns: [{
        title: 'Number',
        data: 'Number',
        className: 'dt-center'
      },
      {
        title: 'Seating Capacity',
        data: 'SeatingCapacity',
        className: 'dt-center'
      },
      {
        title: 'View',
        render: function (data: any, type: any, full: any) {

          return "<img  id='view' tableId='" + full.Id + "' class='btn view'/ src='assets/icons/view.png' >";
        },
        className: 'dt-center'
      }
        , {
        title: 'Edit',
        render: function (data: any, type: any, full: any) {

          return "<img  id='edit' tableId='" + full.Id + "' class='btn edit' src='assets/icons/edit.png' [disabled]='editButtonDisabled'/>";
        },
        className: 'dt-center'
      }
        , {
        title: 'Delete',
        render: function (data: any, type: any, full: any) {
          return "<img  id='delete' tableId='" + full.Id + "' class='btn delete' src='assets/icons/delete.png'/>";
        },
        className: 'dt-center'
      }],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
        
        $('td', row).unbind('click');
        $('td', row).bind('click', () => {
          this.onView((<SelectableTable>data).Id);
        });
        return row;
      }
      ,

    }
  }



  ngOnInit() {
    this.currentTable = new SelectableTable();
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
        this.onDelete(evt.target.getAttribute("tableId"));
      }
      else if (evt.target.id == 'edit') {
        this.onEdit(evt.target.getAttribute("tableId"));
      }
      /*
    else if (evt.target.Id == 'view') {
      this.viewTable(evt.target.getAttribute("tableId"));
    }*/
    });

  }
  refresh() {
    try {


      this.data.getTables()
        .pipe(finalize(() => {
          this.dtTrigger.next();
          this.currentTable = new SelectableTable();
          this.formState.setFormToViewState();
          this.cd.detectChanges();
        }))
        .subscribe((tables: SelectableTable[]) => {
          console.log("Tables fetched");
          this.tables = tables;
          console.log(tables.length);
          var myTable = $('#tableId').DataTable();
          myTable.clear().rows.add(this.tables).draw();

        });
    }
    catch (e) { console.log(e); }
  }


  onCreate() {
    this.currentTable = new SelectableTable();
    this.formState.setFormToAddState();
  }
  onEdit(tableId) {
    var id = tableId;
    this.currentTable = this.tables.find(function (element) { return element.Id == id; })
    if (this.currentTable == null) {
      this.currentTable = new SelectableTable();
    }
    this.formState.setFormToEditState();
  }




  createTable() {
    this.data.createTable(this.currentTable)

      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/tables', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/tables']));
        this.currentTable = new SelectableTable();
        this.formState.setFormToViewState();
      }))

      .subscribe(
        (table) => { console.log("table created:" + JSON.stringify(table)); },
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })

  }

  updateTable() {
    this.data.updateTable(this.currentTable)

      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/tables', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/tables']));


        this.formState.setFormToViewState();
      }))

      .subscribe(
        (table) => { console.log("table created:" + JSON.stringify(table)); },
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })

  }


  onDelete(id) {

    if (!confirm('Do you wish to delete this table?')) return;
    console.log('onDelete:' + id);
    this.data.deleteTable(id)
      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/tables', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/tables']));

        this.formState.setFormToViewState();


      }))

      .subscribe(
        (table) => { console.log("table deleted:" + id); },
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
      this.createTable();
    }
    else if (this.formState.inEdit) {
      this.updateTable();
    }
  }
  onView(id) {
    if (this.formState.inAdd || this.formState.inEdit) {
      if (!confirm("Are you sure you wish to cancel this operation?")) return;
    }

    this.currentTable = this.tables.find(function (element) { return element.Id == id; })

    this.formState.setFormToViewState();
  }


  onCancel(form) {
    this.currentTable = new SelectableTable();
    this.formState.setFormToViewState();
  }


}

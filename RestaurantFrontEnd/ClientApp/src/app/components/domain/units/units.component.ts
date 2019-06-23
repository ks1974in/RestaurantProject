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


import { DataUnitsService } from "../../../services/data-units.service";
import { Unit } from '../../../models/unit';
import { GlobalConstants } from '../../../services/global-constants';

@Component({
  selector: 'app-units',
  templateUrl: './units.component.html',
  styleUrls: ['./units.component.css']
})
export class UnitsComponent implements OnInit, OnDestroy {
  [x: string]: any;
  //Data Table
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<Unit> = new Subject();


  //Models
  units: Unit[] = [];
  currentUnit: Unit;

  listener: any;
  formState: FormState = new FormState();





  constructor(private cd: ChangeDetectorRef,private renderer: Renderer, private router: Router, private httpClient: HttpClient, private formBuilder: FormBuilder, private data: DataUnitsService) { }

  buildOptions() {

    this.dtOptions = {
      ajax: {
        url: GlobalConstants.API +'Unit', dataSrc: "" },
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
        title: 'View',
        render: function (data: any, type: any, full: any) {

          return "<img  id='view' unitId='" + full.Id + "' class='btn view'/ src='assets/icons/view.png' >";
        },
        className: 'dt-center'
      }
        , {
        title: 'Edit',
        render: function (data: any, type: any, full: any) {

          return "<img  id='edit' unitId='" + full.Id + "' class='btn edit' src='assets/icons/edit.png' [disabled]='editButtonDisabled'/>";
        },
        className: 'dt-center'
      }
        , {
        title: 'Delete',
        render: function (data: any, type: any, full: any) {
          return "<img  id='delete' unitId='" + full.Id + "' class='btn delete' src='assets/icons/delete.png'/>";
        },
        className: 'dt-center'
      }],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
       
        $('td', row).unbind('click');
        $('td', row).bind('click', () => {
          this.onView((<Unit>data).Id);
        });
        return row;
      }
      ,

    }
  }



  ngOnInit() {
    this.currentUnit = new Unit();;
    this.buildOptions();
    this.refresh();

  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
    this.listener();
  }


  ngAfterViewInit(): void {
   
    this.listener=this.renderer.listen('document', 'click', (evt) => {

      if (evt.target.id == 'delete') {
        this.onDelete(evt.target.getAttribute("unitId"));
      }
      else if (evt.target.id == 'edit') {
        this.onEdit(evt.target.getAttribute("unitId"));
      }
      /*
    else if (evt.target.Id == 'view') {
      this.viewUnit(evt.target.getAttribute("unitId"));
    }*/
    });

  }
  refresh() {
    try {


      this.data.getUnits()
        .pipe(finalize(() => {
          this.dtTrigger.next();
          this.currentUnit = new Unit();;
          this.formState.setFormToViewState();
          this.cd.detectChanges();
        }))
        .subscribe((units: Unit[]) => {
          console.log("Units fetched");
          this.units = units;
          console.log(units.length);
          console.log(JSON.stringify(units));
          var myTable = $('#tableId').DataTable();
          myTable.clear().rows.add(this.units).draw();

        });
    }
    catch (e) { console.log(e); }
  }


  onCreate() {
    this.currentUnit = new Unit();;
    this.formState.setFormToAddState();
  }
  onEdit(unitId) {
    var id = unitId;
    this.currentUnit = this.units.find(function (element) { return element.Id == id; })
    if (this.currentUnit == null) {
      this.currentUnit = new Unit();;
    }
    this.formState.setFormToEditState();
  }




  createUnit() {
    this.data.createUnit(this.currentUnit)

      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/units', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/units']));
        this.currentUnit = new Unit();
        this.formState.setFormToViewState();
      }))

      .subscribe(
        (unit) => { console.log("unit created:" + JSON.stringify(unit)); },
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })

  }

  updateUnit() {
    this.data.updateUnit(this.currentUnit)

      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/units', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/units']));


        this.formState.setFormToViewState();
      }))

      .subscribe(
        (unit) => { console.log("unit created:" + JSON.stringify(unit)); },
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })

  }


  onDelete(id) {

    if (!confirm('Do you wish to delete this unit?')) return;
    console.log('onDelete:' + id);
    this.data.deleteUnit(id)
      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/units', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/units']));

        this.formState.setFormToViewState();


      }))

      .subscribe(
        (unit) => { console.log("unit deleted:" + id); },
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
      this.createUnit();
    }
    else if (this.formState.inEdit) {
      this.updateUnit();
    }
  }
  onView(id) {
    if (this.formState.inAdd || this.formState.inEdit) {
      if (!confirm("Are you sure you wish to cancel this operation?")) return;
    }

    this.currentUnit = this.units.find(function (element) { return element.Id == id; })

    this.formState.setFormToViewState();
  }


  onCancel(form) {
    this.currentUnit = new Unit();
    this.formState.setFormToViewState();
  }


}

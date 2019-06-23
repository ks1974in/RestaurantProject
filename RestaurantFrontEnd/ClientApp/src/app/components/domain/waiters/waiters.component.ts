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


import { DataUsersService } from "../../../services/data-users.service";
import { DataTablesService } from "../../../services/data-tables.service";
import { DataRolesService } from "../../../services/data-roles.service";

import { Role } from '../../../models/role';
import { Waiter } from '../../../models/waiter';
import { Table } from '../../../models/table';

import { SelectableTable } from '../../../models/selectable-table';
import { GlobalConstants } from '../../../services/global-constants';

@Component({
  selector: 'app-waiters',
  templateUrl: './waiters.component.html',
  styleUrls: ['./waiters.component.css']
})




export class WaitersComponent implements OnInit, OnDestroy {
  [x: string]: any;
  //Data Table
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<Waiter> = new Subject();
  waiters: Waiter[] = [];
  selectedTables: SelectableTable[] = [];
  allTables: SelectableTable[] = [];
  //Models
  roleWaiter: Role;
  currentWaiter: Waiter;

  listener: any;
  formState: FormState = new FormState();

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.Id === c2.Id : c1 === c2;
  }



  constructor(private cd: ChangeDetectorRef,private renderer: Renderer, private router: Router, private httpClient: HttpClient, private formBuilder: FormBuilder, private data: DataUsersService, private dataTables: DataTablesService) { }

  
  selectedWaiterChanged()
  {
    var that = this;
    this.selectedTables.length = 0;
    console.log(JSON.stringify(this.currentWaiter));
    document.getElementsByName('chkSelect').forEach(function (chk: HTMLInputElement, index: number)
    {
      var tableId: string = chk.getAttribute('tableId');
      var waiterTable = that.currentWaiter.WaiterTables.find(x => x.TableId == tableId);
      if (waiterTable != null)
      {
        var table: SelectableTable = that.allTables.find(function (element) { return element.Id == waiterTable.TableId; });
        console.log(JSON.stringify(table));
        chk.checked = true;
        that.selectedTables.push(table);
      }
      else {
        chk.checked = false;
      }
      console.log(index + ':' + tableId + ':' + chk.checked);
      that.cd.detectChanges();

    });
   // (<HTMLInputElement>(document.getElementsByName('txtSelectedSelectedTables')[0])).value = this.listSelectedTables();
    
  }



  listSelectedTables() {
  return this.selectedTables.map(function (table: SelectableTable) {
      return table.Number;
    }).toString();
  
  }
  onSelect(table: SelectableTable, index: number) {
    console.log('Table selected:' + table.Selected + ':index:' + index);
    
    
    var chkBox: HTMLInputElement;
    chkBox = <HTMLInputElement>document.getElementsByName("chkSelect")[index];
    chkBox.checked = !chkBox.checked;
    console.log(chkBox.checked);
    table.Selected = chkBox.checked;
    console.log(table.Selected);
    if (table.Selected) {
      this.selectedTables.push(table);
    }
    else
    {
      this.selectedTables = this.selectedTables.filter(function (value, index, arr) {

        return table.Id!=value.Id;

      });
    }
  }

  buildOptions() {

    this.dtOptions = {
      ajax: {
        url: GlobalConstants.API + 'Table', dataSrc: ""
      },
      pagingType: 'full_numbers',
      pageLength: 3,
      destroy: true,
      columns: [
        {
          title: 'Select',
          render: function (data: any, type: any, full: any) {

            return "<input  name='chkSelect'  tableId='" + full.Id + "' type='checkbox' >";
          },
          className: 'dt-center'
        },
        {
          title: 'Number',
          data: 'Number',
          className: 'dt-center'
        },
        {
          title: 'Seating Capacity',
          data: 'SeatingCapacity',
          className: 'dt-center'
        }],
      rowCallback: (row: Node, data: any[] | Object, index: number) => {
      
        $('td', row).unbind('click');
        $('td', row).bind('click', () => {
          this.onSelect(<SelectableTable>data,index);
        });
        return row;
      }

    
    }
  }



  ngOnInit() {
    
    this.currentWaiter = new Waiter();
    this.buildOptions();
    this.refresh();
    
    
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
   // this.listener();
  }

 


  refresh() {
    try {


      this.dataTables.getTables()
        .pipe(finalize(() => {
          this.data.getUsersByRole('Waiter')
            .pipe(finalize(() => {
              this.dtTrigger.next();
              this.currentWaiter = new Waiter();
              this.formState.setFormToViewState();
              this.cd.detectChanges();
            }))
            .subscribe((waiters: Waiter[]) => {
              console.log("Waiters fetched");
              this.waiters = waiters;
              console.log(waiters.length);
              var myTable = $('#tableId').DataTable();
              myTable.clear().rows.add(this.waiters).draw();

            });
        }))
        .subscribe((tables: SelectableTable[]) => {
          console.log("Tables fetched");
          this.allTables = tables;
          console.log(this.allTables.length);
          
        });


     
         


    


         }
    catch (e) { console.log(e); }
  }


 onEdit() {
    this.formState.setFormToEditState();
  }




 /*
  onEdit(itemId) {
    
    var id = itemId;
    this.currentWaiter = this.waiters.find(function (element) { return element.Id == id; })
    if (this.currentWaiter == null) {
      this.currentWaiter = new Waiter();
    }
    this.formState.setFormToEditState();
  }
  */



  
  updateWaiter() {

    console.log("Selected Waiter Id:"+this.currentWaiter.Id);
    this.dataTables.updateWaiter(this.currentWaiter.Id, this.selectedTables)

      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/waiters', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/waiters']));


        this.formState.setFormToViewState();
      }))

      .subscribe(
        (item) => { console.log("item created:" + JSON.stringify(item)); },
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
   if (this.formState.inEdit) {
      this.updateWaiter();
    }
  }
 

  onCancel(form) {
    this.currentWaiter = new Waiter();
    this.formState.setFormToViewState();
  }


}

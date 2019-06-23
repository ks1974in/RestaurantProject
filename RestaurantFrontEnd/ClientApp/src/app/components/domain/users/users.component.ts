import { Component, OnInit, OnDestroy, Renderer } from '@angular/core';
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
import { DataRolesService } from "../../../services/data-roles.service";

import { Role } from '../../../models/role';
import { User } from '../../../models/user';

import { GlobalConstants } from '../../../services/global-constants';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit, OnDestroy {
  [x: string]: any;
  //Data Table
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<User> = new Subject();
  users: User[] = [];
  roles: Role[] = [];
  //Models

  currentUser: User;

  listener: any;
  formState: FormState = new FormState();

  compareFn(c1: any, c2: any): boolean {
    return c1 && c2 ? c1.Id === c2.Id : c1 === c2;
  }



  constructor(private renderer: Renderer, private router: Router, private httpClient: HttpClient, private formBuilder: FormBuilder, private data: DataUsersService, private dataUsers: DataUsersService, private dataRoles: DataRolesService) { }




  buildOptions() {

    this.dtOptions = {
      ajax: {
        url: GlobalConstants.API +'User', dataSrc: "" },
      pagingType: 'full_numbers',
      pageLength: 3,
      destroy: true,
      columns: [{
        title: 'UserName',
        data: 'UserName',
        className: 'dt-center'
      },
      {
        title: 'First Name',
        data: 'FirstName',
        className: 'dt-center'
        },
        {
          title: 'Last Name',
          data: 'LastName',
          className: 'dt-center'
        },
        {
          title: 'Email Address',
          data: 'EmailAddress',
          className: 'dt-center'
        },

        {
          title: 'Mobile',
          data: 'MobileNumber',
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
          this.onView((<User>data).Id);
        });
        return row;
      }
      ,

    }
  }



  ngOnInit() {

    this.currentUser = new User();
    this.buildOptions();
    this.dataRoles.getRoles()
      .pipe(finalize(() => {
        console.log(this.roles.length + ' roles fetched');
           this.refresh();
          })).subscribe((roles: Role[]) => { this.roles = roles; });
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
      this.viewUser(evt.target.getAttribute("itemId"));
    }*/
    });

  }
  refresh() {
    try {




      this.data.getUsers()
        .pipe(finalize(() => {
          this.dtTrigger.next();
          this.currentUser = new User();
          this.formState.setFormToViewState();
        }))
        .subscribe((users: User[]) => {
          console.log("Users fetched");
          this.users = users;
          console.log(users.length);
          console.log(JSON.stringify(users));
          var myTable = $('#tableId').DataTable();
          myTable.clear().rows.add(this.users).draw();

        });
    }
    catch (e) { console.log(e); }
  }


  onCreate() {
    this.currentUser = new User();
    this.formState.setFormToAddState();
  }
  onEdit(itemId) {
    var id = itemId;
    this.currentUser = this.users.find(function (element) { return element.Id == id; })
    if (this.currentUser == null) {
      this.currentUser = new User();
    }
    this.formState.setFormToEditState();
  }




  createUser() {
  
    this.data.createUser(this.currentUser)

      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/users', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/users']));
        this.currentUser = new User();
        this.formState.setFormToViewState();
      }))

      .subscribe(
        (item) => { console.log("item created:" + JSON.stringify(item)); },
        (err) => { console.log(err); },
        () => {
          console.log("finally");
        })

  }

  updateUser() {
 

    this.data.updateUser(this.currentUser)

      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/users', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/users']));


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
    this.data.deleteUser(id)
      .pipe(finalize(() => {

        this.refresh();
        this.router.navigateByUrl('/users', { skipLocationChange: true }).then(() =>
          this.router.navigate(['/users']));

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
      this.createUser();
    }
    else if (this.formState.inEdit) {
      this.updateUser();
    }
  }
  onView(id) {
    if (this.formState.inAdd || this.formState.inEdit) {
      if (!confirm("Are you sure you wish to cancel this operation?")) return;
    }

    this.currentUser = this.users.find(function (element) { return element.Id == id; })

    this.formState.setFormToViewState();
  }


  onCancel(form) {
    this.currentUser = new User();
    this.formState.setFormToViewState();
  }


}

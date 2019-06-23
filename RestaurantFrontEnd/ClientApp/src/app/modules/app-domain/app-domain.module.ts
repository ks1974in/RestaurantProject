import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FlexLayoutModule } from '@angular/flex-layout';


import { DataTablesModule } from 'angular-datatables';





import { CategoriesComponent } from './../../components/domain/categories/categories.component';
import { ItemsComponent } from './../../components/domain/items/items.component';
import { UnitsComponent } from './../../components/domain/units/units.component';

import { SectionsComponent } from './../../components/domain/sections/sections.component';
import { TablesComponent } from './../../components/domain/tables/tables.component';


import { WaitersComponent } from './../../components/domain/waiters/waiters.component';
import { UsersComponent } from './../../components/domain/users/users.component';




@NgModule({
  declarations: [
    CategoriesComponent,
    ItemsComponent,
    UnitsComponent,
    WaitersComponent,
    UsersComponent,
    SectionsComponent,
    TablesComponent,
  
  ],
  imports: [
    DataTablesModule,
    CommonModule,
    BrowserModule,
    ReactiveFormsModule,
    HttpClientModule,
    FlexLayoutModule.withConfig({
      useColumnBasisZero: false,
      addFlexToParent: false,
    }),
    DataTablesModule,
    BrowserAnimationsModule,
    FormsModule,
    
  ],
  exports: [
    CategoriesComponent,
    ItemsComponent,
    UnitsComponent,
    SectionsComponent,
    TablesComponent,
    WaitersComponent,
    UsersComponent,
  ]
})
export class AppDomainModule { }

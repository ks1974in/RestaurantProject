import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { FlexLayoutModule } from '@angular/flex-layout';



import { MatSidenavModule, MatIconModule, MatToolbarModule, MatListModule, MatNativeDateModule } from '@angular/material';


import { AppRoutingModule } from './app-routing.module';



import { AppComponent } from './app.component';

import { DataTablesModule } from 'angular-datatables';

import { NavComponent } from './components/nav//nav/nav.component';
import { MenuComponent } from './components/nav/menu/menu.component';


import { AboutComponent } from './components/general/about/about.component';
import { ContactComponent } from './components/general/contact/contact.component';
import { HomeComponent } from './components/general/home/home.component';

import { AppDomainModule } from './modules/app-domain/app-domain.module';
import { OrderComponent } from './components/domain/order/order.component';
import { ViewOrdersComponent } from './components/domain/view-orders/view-orders.component';
import { EditOrderComponent } from './components/domain/edit-order/edit-order.component';
import { ViewOrderComponent } from './components/domain/view-order/view-order.component';
import { ViewOrderedItemsComponent } from './components/domain/view-ordered-items/view-ordered-items.component';
import { KitchenScreenComponent } from './components/domain/kitchen-screen/kitchen-screen.component';


declare var $: JQueryStatic;



@NgModule({
  declarations: [
    AppComponent,
    AboutComponent,
    ContactComponent,
    HomeComponent,
    NavComponent,
    MenuComponent,
    OrderComponent,
    ViewOrdersComponent,
    EditOrderComponent,
    ViewOrderComponent,
    ViewOrderedItemsComponent,
    KitchenScreenComponent,
   
   
   
    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FlexLayoutModule.withConfig({
      useColumnBasisZero: false,
      addFlexToParent: false,
    }),
    DataTablesModule.forRoot(),
    BrowserAnimationsModule,
    MatSidenavModule,
    MatIconModule,
    MatToolbarModule,
    MatListModule,
    FormsModule,
    AppDomainModule,
  ],
  exports: [MatSidenavModule, MatIconModule, MatToolbarModule, MatListModule, AppDomainModule],
  entryComponents: [],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

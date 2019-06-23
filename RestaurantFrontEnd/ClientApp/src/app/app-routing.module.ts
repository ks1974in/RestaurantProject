import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { NavComponent } from './components/nav//nav/nav.component';
import { MenuComponent } from './components/nav/menu/menu.component';


import { AboutComponent } from './components/general/about/about.component';
import { ContactComponent } from './components/general/contact/contact.component';
import { HomeComponent } from './components/general/home/home.component';


import { CategoriesComponent } from './components/domain/categories/categories.component';
import { ItemsComponent } from './components/domain/items/items.component';
import { UnitsComponent } from './components/domain/units/units.component';

import { SectionsComponent } from './components/domain/sections/sections.component';
import { TablesComponent } from './components/domain/tables/tables.component';

import { WaitersComponent } from './components/domain/waiters/waiters.component';
import { UsersComponent } from './components/domain/users/users.component';
import { OrderComponent } from './components/domain/order/order.component';
import { ViewOrdersComponent } from './components/domain/view-orders/view-orders.component';
import { EditOrderComponent } from './components/domain/edit-order/edit-order.component';
import { ViewOrderComponent } from './components/domain/view-order/view-order.component';
import { ViewOrderedItemsComponent } from './components/domain/view-ordered-items/view-ordered-items.component';
import { KitchenScreenComponent } from './components/domain/kitchen-screen/kitchen-screen.component';



const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'about', component: AboutComponent },
  { path: 'contact', component: ContactComponent },


  { path: 'categories', component: CategoriesComponent },
  { path: 'items', component: ItemsComponent },
  { path: 'units', component: UnitsComponent },

  { path: 'sections', component: SectionsComponent },
  { path: 'tables', component: TablesComponent },

  { path: 'users', component: UsersComponent },
  { path: 'waiters', component: WaitersComponent },

  { path: 'createOrders', component: OrderComponent },
  { path: 'viewOrders', component: ViewOrdersComponent },
  { path: 'editOrder/:orderId', component: EditOrderComponent },
  { path: 'viewOrder/:orderId', component: ViewOrderedItemsComponent},
  { path: 'kitchenScreen', component: KitchenScreenComponent },

];


@NgModule({
  imports: [RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }

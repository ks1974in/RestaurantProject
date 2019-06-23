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
import { DataOrdersService } from '../../../services/data-orders-service.service';
import { DataCategoriesService } from '../../../services/data-categories.service';
import { Category } from '../../../models/category';
import { FormState } from '../../../utility-classes/form-state';
import { ScreenModel } from '../../../models/screen-model';


@Component({
  selector: 'app-kitchen-screen',
  templateUrl: './kitchen-screen.component.html',
  styleUrls: ['./kitchen-screen.component.css']
})
export class KitchenScreenComponent implements OnInit, OnDestroy {

  constructor(private renderer: Renderer, private route: ActivatedRoute, private router: Router, private data: DataOrdersService, private dataCategories: DataCategoriesService) { }
  categories: string[];
  screenData: string[][] = [];
  tables: string[] = [];
  listener: any;
  formState: FormState = new FormState();
  ngOnInit() {
    this.refresh();
  }

  ngOnDestroy(): void {
   
    this.listener();
  }


  ngAfterViewInit(): void {

    this.listener = this.renderer.listen('document', 'click', (evt) => {

      if (evt.target.id == 'delete') {
        this.onSelect(evt.target.getAttribute("orderId"));
      }
    });

  }
  onSelect(orderId)
  {
    console.log('order selected:' + orderId);

  }
  refresh() {
   
    this.data.getKitchenScreenData(new Date())
      .pipe(finalize(() => { console.log(JSON.stringify(this.categories)); console.log(JSON.stringify(this.screenData)); })).subscribe((screenData: ScreenModel) => { this.screenData = screenData.Data; this.categories = screenData.Categories; this.tables = screenData.Tables;});
  }
}

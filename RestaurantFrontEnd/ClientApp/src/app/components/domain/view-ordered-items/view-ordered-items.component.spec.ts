import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewOrderedItemsComponent } from './view-ordered-items.component';

describe('ViewOrderedItemsComponent', () => {
  let component: ViewOrderedItemsComponent;
  let fixture: ComponentFixture<ViewOrderedItemsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewOrderedItemsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewOrderedItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

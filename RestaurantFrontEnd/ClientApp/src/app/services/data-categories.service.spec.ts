import { TestBed } from '@angular/core/testing';

import { DataCategoriesService } from './data-categories.service';

describe('DataCategoriesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DataCategoriesService = TestBed.get(DataCategoriesService);
    expect(service).toBeTruthy();
  });
});

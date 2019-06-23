import { TestBed } from '@angular/core/testing';

import { DataOrdersServiceService } from './data-orders-service.service';

describe('DataOrdersServiceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DataOrdersServiceService = TestBed.get(DataOrdersServiceService);
    expect(service).toBeTruthy();
  });
});

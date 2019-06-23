import { TestBed } from '@angular/core/testing';

import { DataUnitService } from './data-unit.service';

describe('DataUnitService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DataUnitService = TestBed.get(DataUnitService);
    expect(service).toBeTruthy();
  });
});

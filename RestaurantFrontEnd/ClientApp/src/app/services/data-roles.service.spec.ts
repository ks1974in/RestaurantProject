import { TestBed } from '@angular/core/testing';

import { DataRolesService } from './data-roles.service';

describe('DataRolesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DataRolesService = TestBed.get(DataRolesService);
    expect(service).toBeTruthy();
  });
});

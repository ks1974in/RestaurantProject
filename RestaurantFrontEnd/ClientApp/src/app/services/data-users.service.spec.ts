import { TestBed } from '@angular/core/testing';

import { DataUsersService } from './data-users.service';

describe('DataUsersService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DataUsersService = TestBed.get(DataUsersService);
    expect(service).toBeTruthy();
  });
});

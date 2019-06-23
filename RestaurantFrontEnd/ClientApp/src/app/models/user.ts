import { Entity } from './entity';
import { Role } from './role';

export class User extends Entity{
  UserName: string;
  FirstName: string;
  LastName: string;
  Password: string;
  EmailAddress: string;
  MobileNumber: string;
  Enabled: boolean;
  Locked: boolean;
  Role: Role;
  CreatedOn: Date;
  Name: string;
  getName() {
    return this.constructor.name;
  }
  constructor(params: User = {} as User)
  {
    super(params);
    this.CreatedOn = params.CreatedOn;
    this.UserName = params.UserName;
    this.FirstName = params.FirstName;
    this.LastName = params.LastName;
    this.EmailAddress = params.EmailAddress;
    this.MobileNumber = params.MobileNumber;
    this.Enabled = params.Enabled;
    this.Locked = params.Locked;
    this.Password = params.Password;
    this.Name = this.FirstName + ' ' + this.LastName;
  }
  
}

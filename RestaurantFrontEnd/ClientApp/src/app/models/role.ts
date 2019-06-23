import { Entity } from "./entity";

export class Role extends Entity{
  Name: string;
  getName() {
    return this.constructor.name;
  }
  constructor(params: Role = {} as Role)
  {
    super(params);
    this.Name = params.Name;
  }
}

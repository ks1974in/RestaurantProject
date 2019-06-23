import { Entity } from "./entity";

export class Unit extends Entity{
  Name: string;
  Code: string;
  getName() {
    return this.constructor.name;
  }
  constructor(params : Unit = {} as Unit) {
    super(params);
    this.Name = params.Name;
    this.Code = params.Code;
  }
}

import { Entity } from "./entity";

export class Category  extends Entity{
  Code: string;
  Name: string;
  Description: string;
  getName() {
    return this.constructor.name;
  }
  constructor(params: Category={ } as Category)
  {
    super(params);
    this.Name = params.Name;
    this.Code = params.Code;
    this.Description = params.Description;

  }
}

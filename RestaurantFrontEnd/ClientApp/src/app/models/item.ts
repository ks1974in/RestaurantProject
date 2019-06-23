import { Entity } from "./entity";

import { Category } from './category';
import { Unit } from './unit';
export class Item extends Entity{
  
  Name: string;
  Code: string;
  Category?: Category;
  Unit?: Unit;
  Price: number;
  Description: string; 
  Available: boolean;
  getName() {
    return this.constructor.name;
  }
  constructor(params: Item = {} as Item)
  {
    super(params);
    this.Name = params.Name;
    this.Code = params.Code;
    this.Category = params.Category;
    this.Unit = params.Unit;
    this.Price = params.Price; 
    this.Description = params.Description;
    this.Available = params.Available;
  }
}

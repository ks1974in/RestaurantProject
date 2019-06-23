import { Entity } from "./entity";
import { WaiterTable } from './waiter-table';

export class Table extends Entity{
  Number: string;
  SeatingCapacity: number;
  Occupied: boolean;
  WaiterTables: WaiterTable[];
  getName() {
    return this.constructor.name;
  }
  constructor(params: Table = {} as Table)
  {
    super(params);
    this.Number = params.Number;
    this.SeatingCapacity = params.SeatingCapacity;
    this.Occupied = params.Occupied;
    this.WaiterTables = params.WaiterTables;
  }

}

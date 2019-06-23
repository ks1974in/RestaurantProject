import { User } from "./user";
import { WaiterTable } from "./waiter-table";
export class Waiter extends User{
  WaiterTables: WaiterTable[];
 
  constructor(params: Waiter = {} as Waiter)
  {
    super(params);
    this.WaiterTables = params.WaiterTables;
    
  }
  
}

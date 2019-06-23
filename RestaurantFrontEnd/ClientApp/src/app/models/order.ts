import { Entity } from "./entity";
import { Waiter } from './waiter';
import { Table } from './table';
import { OrderedItem } from './ordered-item';
//import { NgDate } from '../utility-classes/ng-date';
export class Order extends Entity{
  Waiter: Waiter;
  Table: Table;
  OrderNumber: string;
  Date: Date;
  Completed: boolean=false;
  Billed: boolean=false;
  Amount: number=0;
  Taxes: number=0;
  Subtotal: number=0;
  Discount: number=0;
  Total: number=0;
  Remarks: string;
  Items: OrderedItem[];
  constructor(params: Order = {} as Order)
  {
    super(params);
    this.Waiter = params.Waiter;
    this.Table = params.Table;
    this.OrderNumber = params.OrderNumber;
    this.Date =params.Date;
    this.Completed = params.Completed;
    this.Billed = params.Billed;
    this.Amount = params.Amount;
    this.Taxes = params.Taxes;
    this.Subtotal = params.Subtotal;
    this.Discount = params.Discount;
    this.Total = params.Total;
    this.Remarks = params.Remarks;
    this.Items = params.Items;
  }
}

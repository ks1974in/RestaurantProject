import { Entity } from "./entity";
import { Order } from "./order";
import { Item } from "./item";
export class OrderedItem extends Entity{
  Order: Order;
  Item: Item;
  Quantity: number;
  Price: number;
  Subtotal: number;
  Remarks: number;

  constructor(params: OrderedItem = {} as OrderedItem)
  {
    super(params);
    this.Order = params.Order;
    this.Item = params.Item;
    if (params.Price == null && params.Item != null) { this.Price = params.Item.Price; } else { this.Price = params.Price; }
    this.Quantity = params.Quantity;
    this.Subtotal = this.Price * this.Quantity;
    this.Remarks = params.Remarks;

  }
}

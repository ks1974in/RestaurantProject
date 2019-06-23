import { Entity } from './entity';

export class WaiterTable extends Entity{
  WaiterId: string;
  TableId: string;
  constructor(params: WaiterTable = {} as WaiterTable)
  {
    super(params);
    this.Id = params.Id;
    this.WaiterId = params.WaiterId;
    this.TableId = params.TableId;
  }
}

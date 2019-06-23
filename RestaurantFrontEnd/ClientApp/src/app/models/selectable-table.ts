import { Table } from './table';
export class SelectableTable extends Table{
  Selected: boolean = false;
  constructor(params: SelectableTable = {} as SelectableTable) { super(params); this.Selected = false; }
  public toString = (): string => {
    return this.Number;
  }
}

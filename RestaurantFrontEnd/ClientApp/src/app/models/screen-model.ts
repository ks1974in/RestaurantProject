import { Category } from './category';

export class ScreenModel {
  Categories: string[];
  Data: string[][];
  Tables: string[];
  constructor(params: ScreenModel = {} as ScreenModel)
  {
    this.Categories = params.Categories;
    this.Data = params.Data;
    this.Tables = params.Tables;
  }
}

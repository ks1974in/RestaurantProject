export class HateosLink {
  Href : string;
  Rel : string;
  Action : string;
  Types: string[];
  constructor(params: HateosLink = {} as HateosLink)
  {
    this.Href = params.Href;
    this.Rel = params.Rel;
    this.Action = params.Action;
    this.Types = params.Types;

  }
}

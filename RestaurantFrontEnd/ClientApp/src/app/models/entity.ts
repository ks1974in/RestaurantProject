import { HateosLink } from './hateos-link';
export class Entity {
  Id: string;
  Links: HateosLink[];
  getName() {
    return this.constructor.name;
  }
  constructor(params: Entity = {} as Entity)
  {
    this.Id = params.Id;
    this.Links = params.Links;
  }
}

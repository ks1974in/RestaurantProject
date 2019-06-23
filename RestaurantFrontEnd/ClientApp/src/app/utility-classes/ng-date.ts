import * as moment from 'moment';

export class NgDate {

  date: any;

  constructor(params: any = {} as any) {
    if (params.date == null)
      this.date = new Date();
    else
      this.date = params.date;
  }

  set dateInput(e) {
    if (e != "") {
      var momentDate = moment(e, moment.ISO_8601).toDate();
      this.date = momentDate;
    }
    else {
      this.date = null;
    }
  }

  get dateInput() {
    if (this.date == null) {
      return "";
    }

    var stringToReturn = moment(this.date).format().substring(0, 10);
    return stringToReturn;
  }
}

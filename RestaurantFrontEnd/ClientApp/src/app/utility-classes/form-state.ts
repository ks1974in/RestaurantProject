export class FormState {
  //Form State

  inEdit: boolean = false;
  inAdd: boolean = false;
  inView: boolean = true;

  submitted = false;
  success = false;
  newButtonDisabled: boolean = false;
  editButtonDisabled: boolean = false;
  cancelButtonDisabled: boolean = false;
  saveButtonDisabled: boolean = false;
  disabledActionButtonClass: string = "btn action-button-disabled";
  actionButtonClass: string = "btn action-button";
  isDisabled: boolean = false;


  setFormToAddState() {
   
    this.inEdit = false;
    this.inAdd = true;
    this.isDisabled = false;
    this.newButtonDisabled = true;
    this.editButtonDisabled = true;
    this.cancelButtonDisabled = false;
    this.saveButtonDisabled = false;

  }
  setFormToEditState() {
    this.inEdit = true;
    this.inAdd = false;
    this.isDisabled = false;
    this.newButtonDisabled = true;
    this.editButtonDisabled = true;
    this.cancelButtonDisabled = false;
    this.saveButtonDisabled = false;
  }
  setFormToViewState() {
    this.submitted = false;
    this.isDisabled = true;
    this.inAdd = false;
    this.inEdit = false;
    this.inView = true;
    this.newButtonDisabled = false;
    this.editButtonDisabled = false;
    this.cancelButtonDisabled = true;
    this.saveButtonDisabled = true;
  }
}

import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from "primeng/api";
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-custom-table',
  templateUrl: './custom-table.component.html',
  styleUrls: ['./custom-table.component.css']
})
export class CustomTableComponent implements OnInit {
  @Input() tableData: any;
  @Input() tableColumns: any;
  @Output() downloadPdf = new EventEmitter<string>();
  @Output() RefreshList = new EventEmitter<boolean>();
  @Input() hasPermission: boolean = false;
  @ViewChild('closeModal') closeModal: ElementRef | undefined;
  isShowLoader: boolean = false;
  loading: boolean = false;
  pdfForm: any;
  isClickEdit: boolean = false;
  pdfTitle: string = '';

  constructor(private fb: FormBuilder,
    private _userService: UserService,
    private _tosterService: ToastrService) {
    this.pdfForm = this.fb.group({
      id: ['new'],
      name: ['', [Validators.required]],
      description: ['', [Validators.required]],
      price: ['', [Validators.required]],
      pdfLink: ['', [Validators.required]]
    })
  }
  get Name() {
    return this.pdfForm.get("name");
  }
  get Description() {
    return this.pdfForm.get("description");
  }
  get Price() {
    return this.pdfForm.get("price");
  }
  get Link() {
    return this.pdfForm.get("pdfLink");
  }

  ngOnInit() {
  }


  onClickDownload(value: any) {
    this.downloadPdf.emit(value);
  }
  onClickAddNew() {
    this.resetFormData();
    this.isClickEdit = false;
    this.pdfTitle = "Add New PDF";
  }
  onClickEdit(value: any) {
    this.resetFormData();
    this.isClickEdit = true;
    this.pdfTitle = "Edit PDF Details";
    this.pdfForm.patchValue({
      id: value.id,
      name: value.name,
      description: value.description,
      price: value.price / 100,
      pdfLink: value.pdfLink
    })
  }
  onClickDelete(value: any) {
    if (confirm("Are you sure to delete " + value.name)) {
      console.log("Implement delete functionality here");
    }
  }
  onSubmit() {
    this.showLoader();
    let pdfDetails = this.pdfForm.value;
    if (this.isClickEdit) {
      this._userService.updatePdf(pdfDetails).subscribe({
        next: (res) => {

        },
        error: (err) => {
          this.hideLoader();
          this._tosterService.error("Error while updating. Please Try again", "Error")
        },
        complete: () => {
          this.hideLoader();
          this._tosterService.success("Successfully updated", "Success")
          this.refreshList();
        }
      })
    }
    else {
      this._userService.addNewPdf(pdfDetails).subscribe({
        next: (res) => {

        },
        error: (err) => {
          this._tosterService.error("Error while saving. Please Try again", "Error")
          this.hideLoader();
        },
        complete: () => {
          this.hideLoader();
          this._tosterService.success("Successfully saved", "Success")
          this.refreshList();
        }
      })
    }
  }
  resetFormData() {
    this.pdfForm.reset();
  }
  showLoader() {
    this.isShowLoader = true;
  }
  hideLoader() {
    this.isShowLoader = false;
  }
  refreshList() {
    if (this.closeModal)
      this.closeModal.nativeElement.click();
    this.RefreshList.emit(true);
  }
}

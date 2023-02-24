import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {

  isShowLoader: boolean = false;
  contactForm: any;
  constructor(private userService: UserService,
              private _tosterService: ToastrService,
              private fb: FormBuilder) 
  {
    this.contactForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      message: ['', [Validators.required, Validators.minLength(10)]]
    })
  }
  get Email() {
    return this.contactForm.get("email");
  }
  get Message() {
    return this.contactForm.get("message");
  }
  ngOnInit(): void {
  }
  resetForm() {
    this.contactForm.reset();
  }
  onSubmit(){
    let contactDetails = this.contactForm.value;
    this.showLoader();
    this.userService.userContact(contactDetails).subscribe({
      next: (res) => {

      },
      error: (err) => {
        this._tosterService.error("Not able to send your Query. Please Try again!", "Error");
        this.hideLoader();
      },
      complete: () => {
        this._tosterService.success("Your query is captured", "Success");
        this.hideLoader();
        this.resetForm();
      }
    });
  }

  showLoader(){
    this.isShowLoader = true;
  }
  hideLoader(){
    this.isShowLoader = false;
  }
}

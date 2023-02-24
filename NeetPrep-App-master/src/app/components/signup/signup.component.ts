import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { RedirectService } from 'src/app/shared/services/redirect.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  isShowLoader: boolean = false;

  registerForm: any;
  isSubmitted: boolean = false;
  constructor(private fb: FormBuilder,
    private _userService: UserService,
    private _redirectService: RedirectService,
    private _tosterService: ToastrService) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(4)]],
      otp: ['', [Validators.pattern(/^[0-9]{6}$/)]]
    })
  }

  get Email() {
    return this.registerForm.get("email");
  }
  get Password() {
    return this.registerForm.get("password");
  }
  get OTP() {
    return this.registerForm.get("otp");
  }

  onSubmit() {
    let userDetails = this.registerForm.value;
    this.showLoader();
    userDetails.password = "";
    this._userService
      .generateOTP(userDetails)
      .subscribe({
        next: (res) => {
          console.log(res)
        },
        error: (err) => {
          console.log(err)
          this.hideLoader();
          this._tosterService.error(!err.error.message? "Something went wrong. Please try again": err.error.message,'Error');
        },
        complete: () => {
          this.isSubmitted = true
          this.hideLoader();
          this._tosterService.success("A verification code has been sent to your email. Please verify!","Success");
        }
      })
  }

  onVerifySubmit() {
    let userDetails = this.registerForm.value;
    this.showLoader();
    this._userService
      .register(userDetails)
      .subscribe({
        next: (res) => {
          console.log(res);
        },
        error: (err) => {
          console.log(err)
          this.isSubmitted = false
          this.hideLoader();
          this._tosterService.error(!err.error.message ? "Something went wrong. Please try again": err.error.message,'Error');
        },
        complete: () => {
          this._redirectService.redirectTo("login");
          this.hideLoader();
          this._tosterService.success("User successfully registerd. Please login!","Success")
        }
      })
  }

  showLoader() {
    this.isShowLoader = true;
  }
  hideLoader() {
    this.isShowLoader = false;
  }
  ngOnInit(): void { }
}

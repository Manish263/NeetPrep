import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { RedirectService } from 'src/app/shared/services/redirect.service';
import { TokenService } from 'src/app/shared/services/token.service';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  isShowLoader: boolean = false;
  
  isForgotPassword: boolean = false;
  isForgotPassSubmit: boolean = false;
  loginForm: any;
  forgotPassForm: any;
  changePassForm: any;

  constructor(private fb: FormBuilder,
    private _userService: UserService,
    private _tokenService: TokenService,
    private _redirectService: RedirectService,
    private _tosterService: ToastrService) {

    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(4)]]
    })

    this.forgotPassForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    })

    this.changePassForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      otp: ['', [Validators.required, Validators.pattern(/^[0-9]{6}$/)]],
      password: ['', [Validators.required, Validators.minLength(4)]]
    })
  }

  get Email() {
    return this.loginForm.get("email");
  }
  get Password() {
    return this.loginForm.get("password");
  }
  get ForgotEmail() {
    return this.forgotPassForm.get("email");
  }
  get changePassEmail() {
    return this.changePassForm.get("email");
  }
  get ChangePassOTP() {
    return this.changePassForm.get("otp");
  }
  get NewPassword() {
    return this.changePassForm.get("password");
  }

  forgotPass() {
    this.isForgotPassword = !this.isForgotPassword;
  }

  onSubmit() {
    let userDetails = this.loginForm.value;
    this.showLoader();
    this._userService
      .login(userDetails)
      .subscribe({
        next: (res) => {
          console.log(res)
          this._tokenService.setToken(res.token);
        },
        error: (err) => {
          console.log(err)
          this.hideLoader();
          this._tosterService.error(!err.error.message? "Something went wrong. Please try again": err.error.message,'Error');
        },
        complete: () =>{
          this._redirectService.AutoRedirect();
          this.hideLoader();
          this._tosterService.success("Login successfully!","Success");
        }
      })
  }

  onForgotSubmit() {
    let userDetails = this.forgotPassForm.value;
    this.showLoader();
    this._userService
    .genForgotPassOTP(userDetails)
    .subscribe({
      next: (res) => {
        console.log(res);
      },
      error: (err) => {
        console.log(err);
        this.hideLoader();
        this._tosterService.error(!err.error.message ? "Something went wrong. Please try again": err.error.message,'Error');
      },
      complete: () => {
        this.changePassEmail.patchValue(userDetails.email);
        this.isForgotPassSubmit = true;
        this.hideLoader();
        this._tosterService.success("A verification code has been sent to your email. Please verify!","Success");
      }
    })
  }

  onChangePassSubmit() {
    let userDetails = this.changePassForm.value;
    this.showLoader();
    this._userService
    .changePassword(userDetails)
    .subscribe({
      next: (res) => {
        console.log(res);
      },
      error: (err) => {
        console.log(err);
        this.hideLoader();
        this._tosterService.error(!err.error.message ? "Something went wrong. Please try again": err.error.message,'Error');
      },
      complete: () => {
        this.resetComponent();
        this.hideLoader();
        this._tosterService.success("Password successfully changed. Please try login!","Success");
      }
    })
  }

  resetComponent(){
    this.isForgotPassword = false;
    this.isForgotPassSubmit = false;
    this.loginForm.reset();
    this.forgotPassForm.reset();
    this.changePassForm.reset();
  }

  showLoader(){
    this.isShowLoader = true;
  }
  hideLoader(){
    this.isShowLoader = false;
  }
  ngOnInit(): void { }
}

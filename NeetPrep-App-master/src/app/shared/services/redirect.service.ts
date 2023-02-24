import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class RedirectService {

  constructor(private router: Router,
              private _tokenService: TokenService) { }

  redirectTo(route: string){
    switch (route) {
      case "home":{
        this.router.navigateByUrl('/home');
        break;
      }
      case "profile":{
        this.router.navigateByUrl('/profile');
        break;
      }
      case "contact":{
        this.router.navigateByUrl('/contact');
        break;
      }
      case "dashboard":{
        this.router.navigateByUrl('/dashboard');
        break;
      }
      case "login":{
        this.router.navigateByUrl('/login')
        break;
      }
      case "signup":{
        this.router.navigateByUrl('/signup')
        break;
      }
    }
  }

  AutoRedirect(){
    switch (this._tokenService.getRole()) {
      case "REG":{
        if (this._tokenService.isTokenExpired())
          this.router.navigateByUrl('/login');
        else
          this.router.navigateByUrl('/dashboard');
        break;
      }
      case "":{
        this.router.navigateByUrl('/');
        break;
      }
      default:{
        this.router.navigateByUrl('/');
        break;
      }
    }
  }
}

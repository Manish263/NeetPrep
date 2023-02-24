import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { RedirectService } from '../services/redirect.service';
import { TokenService } from '../services/token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationGuard implements CanActivate {

  constructor(private _tokenService: TokenService,
    private _redirectService: RedirectService) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this._tokenService.isTokenExpired() && this._tokenService.getRole() != "REG") {
      this._redirectService.AutoRedirect();
      console.log("Token expired");
      return false;
    }
    return true;
  }
}

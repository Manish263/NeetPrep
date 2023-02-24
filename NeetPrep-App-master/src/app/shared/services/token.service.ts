import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  constructor() { }
  private role = '';
  private userName = '';
  private setRole(role : string){
    this.role = role;
  }

  private decodeToken(token : string | null) {
    if(token != null)
      return JSON.parse(window.atob(token.split('.')[1]))
  }

  getRole() : string{
    return this.role;
  }

  getUserEmail() : string {
    return this.userName;
  }

  doesTokenExist(key : string){
    var token = localStorage.getItem(key);
    if (token == null)
        return false;
    return true;
  }

  isTokenExpired(): boolean{
    if(this.doesTokenExist('jwt')){
      var decodedtoken = this.decodeToken(this.getToken('jwt'));
      var expiresAt = decodedtoken.exp;
      var now = (new Date().getTime())/1000;
      var timeLeft = expiresAt-now;
      if(timeLeft > 0)
        return false;
    }
    return true;
  }

  getToken(key : string) {
    return localStorage.getItem(key);
  }

  removeToken(key : string){
    localStorage.removeItem(key);
    this.setRole('');
  }

  setToken(token : string){
    localStorage.setItem('jwt', token);
    var decodedtoken = this.decodeToken(token);
    this.setRole(decodedtoken.Role);
  }
}

import { Component, OnInit } from '@angular/core';
import { RedirectService } from '../../services/redirect.service';
import { TokenService } from '../../services/token.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {
  isUserLoggedIn: boolean = false;
  constructor(private _tokenService: TokenService,
              private _redirectService: RedirectService) { }

  ngOnInit(): void {
    this.setNavBar();
  }

  setNavBar(){
    if (this._tokenService.isTokenExpired() && this._tokenService.getRole() != "REG"){
      this.isUserLoggedIn = false;
    }
    else
      this.isUserLoggedIn = true;
  }

  logout(){
    this._tokenService.removeToken('jwt');
    this._redirectService.AutoRedirect();
  }
}

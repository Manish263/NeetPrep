import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse
} from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { TokenService } from '../services/token.service';
import { Router } from '@angular/router';
import { RedirectService } from '../services/redirect.service';

@Injectable()
export class HttpInterceptorInterceptor implements HttpInterceptor {
  omitCalls = ['auth'];
  skipInterceptor = false;
  constructor(private _tokenService: TokenService,
    private _redirectService: RedirectService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {

    if (!this._tokenService.isTokenExpired()) {
      const tokenizedReq = request.clone({
        headers: request.headers.set('Authorization', 'Bearer ' + this._tokenService.getToken('jwt'))
      });
      return next.handle(tokenizedReq);
    }
    return next.handle(request);
    
  }
}

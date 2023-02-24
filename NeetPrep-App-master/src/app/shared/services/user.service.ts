import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  _url = "https://localhost:44320/api/"
  constructor(private _http: HttpClient) { }

  getBlobUrl(url: string): Observable<Blob> {
    return this._http.get(url, {
      responseType: 'blob'
    })
  }

  generateOTP(data: any) {
    return this._http.post<any>(this._url + "AuthApi/generateOTP", data);
  }
  register(data: any) {
    return this._http.post<any>(this._url + "AuthApi/register", data);
  }

  login(data: any) {
    return this._http.post<any>(this._url + "AuthApi/login", data);
  }
  genForgotPassOTP(data: any) {
    return this._http.post<any>(this._url + "AuthApi/genForgotPassOTP", data);
  }
  changePassword(data: any) {
    return this._http.post<any>(this._url + "AuthApi/changePassword", data);
  }
  getPdfDetails() {
    return this._http.get<any>(this._url + "Dashboard/getPdfDetails")
  }

  createOrder(Id: any) {
    return this._http.post<any>(this._url + "Dashboard/createOrder?Id="+Id, {});
  }
  updateOrder(data: any) {
    var PaymentRequest =  {
      OrderId: data.razorpay_order_id,
      PaymentId: data.razorpay_payment_id,
      Signature: data.razorpay_signature
    }
    return this._http.post<any>(this._url + 'Dashboard/updateOrder', PaymentRequest);
  }

  paymentFailed(data:any) {
    return this._http.post<any>(this._url + 'Dashboard/paymentFailed', data);
  }
  userContact(data:any) {
    return this._http.post<any>(this._url + 'Dashboard/userContact', data);
  }
  addNewPdf(data:any) {
    return this._http.post<any>(this._url + 'Dashboard/addNewPdf', data);
  }
  updatePdf(data:any) {
    return this._http.post<any>(this._url + 'Dashboard/updatePdf', data);
  }
  deletePdf(Id:any) {
    return this._http.post<any>(this._url + 'Dashboard/updatePdf?Id='+Id, {});
  }
  getEditPermission(){
    return this._http.get<any>(this._url + 'Dashboard/getEditPermission')
  }
}
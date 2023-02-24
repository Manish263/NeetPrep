import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Component, ElementRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { TokenService } from 'src/app/shared/services/token.service';
import { UserService } from 'src/app/shared/services/user.service';

declare var Razorpay: any;
declare var require: any
const FileSaver = require('file-saver');

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {
  isShowLoader: boolean = false;

  paymentId: string = "";
  orderId: string = "";
  paymentError: string = "";
  showPaymentStatus: boolean = false
  isPaymentSuccess: boolean = false
  hasEditPermission: boolean = false;

  tblColDetails = ["Name", "Discription", "Action"];
  pdfDetails: any;
  createOrderResponse: any;

  constructor(private userService: UserService,
    private _tosterService: ToastrService) {
  }

  ngOnInit(): void {
    this.GetPdfDetails();
    this.GetandSetEditPermission()
  }

  GetPdfDetails() {
    this.showLoader();
    this.userService.getPdfDetails().subscribe({
      next: (res) => {
        this.pdfDetails = res.pdfDetails;
      },
      error: (err) => {
        this.hideLoader();
      },
      complete: () => {
        this.hideLoader();
      }
    });
  }

  GetandSetEditPermission() {
    this.userService.getEditPermission().subscribe({
      next: (res) => {
        if (res && res.hasEditPermission == true)
          this.hasEditPermission = true;
        else
          this.hasEditPermission = false;
      },
      error: (err) => {
        this.hasEditPermission = false;
      }
    });
  }

  onClickDownload(data: any) {
    console.log("Comming from child", data);
    this.showPaymentStatus = true;
    this.createOrder(data)
  }

  RefreshList(isRefresh: boolean){
    if (isRefresh == true)
      this.ngOnInit();
  }
  options = {
    "key": "rzp_test_AJTr4XAP2FPexJ",
    "amount": "433",
    "currency": 'INR',
    "name": "Java Chinna",
    "description": "Web Development",
    "image": "../../../assets/Images/bg_1.jpg",
    "order_id": "order_JaluYAy3fgkYpl",
    "handler": function (response: any) {
      var event = new CustomEvent("payment.success",
        {
          detail: response,
          bubbles: true,
          cancelable: true
        }
      );
      window.dispatchEvent(event);
    },
    // "modal": {
    //   // We should prevent closing of the form when esc key is pressed.
    //   "escape": false,
    //   "ondismiss" : (() => {
    //     // handle the case when user closes the form while transaction is in progress
    //     console.log('Transaction cancelled.');
    //   })
    // },
    "prefill": {
      "email": "A2g.b"
    },
    "notes": {
    },
    "theme": {
      "color": "#3399cc"
    }
  };

  createOrder(data: any): void {
    this.showLoader();
    this.userService.createOrder(data.id).subscribe({
      next: (res) => {
        if (res)
          this.createOrderResponse = res.orderResponse;

        if (this.createOrderResponse && this.createOrderResponse.isAvailable) {
          this.DownloadPdf(this.createOrderResponse.pdfLink, this.createOrderResponse.orderName);
          this.hideLoader()
        }
        else if (this.createOrderResponse) {
          this.orderId = this.createOrderResponse.orderId;
          this.options.key = this.createOrderResponse.secretKey;
          this.options.order_id = this.createOrderResponse.orderId;
          this.options.amount = this.createOrderResponse.price;
          this.options.currency = this.createOrderResponse.currency;
          this.options.name = this.createOrderResponse.orderName;
          this.options.description = this.createOrderResponse.orderDescription;
          this.options.prefill.email = this.createOrderResponse.email;
          var rzp = new Razorpay(this.options);
          rzp.open();
          this.hideLoader()

          rzp.on('payment.failed', (response: any) => {
            console.log("failed::::", response)
            response.cancelBubble = true;
            this._tosterService.error("Order Id: " + response.error.metadata.order_id + " Payment Id: " + response.error.metadata.payment_id, 'Payment failed');
            var errorDetails = {
              ErrorCode: response.error.code,
              Description: response.error.description,
              Source: response.error.source,
              Step: response.error.step,
              Reason: response.error.reason,
              OrderId: response.error.metadata.order_id,
              PaymentId: response.error.metadata.payment_id
            }
            this.paymentError = response.error.reason;
            this.showLoader()
            this.userService.paymentFailed(errorDetails).subscribe({
              next: (res) => {
                console.log("payment error success stored")
              },
              error: (err) => {
                console.log("payment error fsiled stored")
                this.hideLoader()
              },
              complete: () => {
                this.hideLoader()
              }
            });
          });
        }
      },
      error: (err) => {
        this._tosterService.error("Some Error Occured while processing your request", 'Error')
        this.hideLoader()
      },
      complete: () => {

      }
    });
  }

  DownloadPdf(url: any, name: any) {
    const pdfUrl = url;
    const pdfName = name;
    FileSaver.saveAs(pdfUrl, name + ".pdf");
  }
  // DownloadPdf(url:any, name:any): void {
  //   this.userService
  //     .getBlobUrl(url)
  //     .subscribe(blob => {
  //       const a = document.createElement('a')
  //       const objectUrl = URL.createObjectURL(blob)
  //       a.href = objectUrl
  //       a.download = name + '.zip';
  //       a.click();
  //       URL.revokeObjectURL(objectUrl);
  //     })
  // }

  @HostListener('window:payment.success', ['$event'])
  onPaymentSuccess(event: any): void {
    this.showLoader();
    this.userService.updateOrder(event.detail).subscribe({
      next: (res) => {
        event.cancelBubble = true;
        if (res.isSuccess) {
          this.paymentId = res.paymentResponse.paymentId;
          this.isPaymentSuccess = true;
        }
        else {
          this.paymentError = res.message;
          this._tosterService.error("Your Order Id: " + this.orderId, 'Payment failed', {
            timeOut: 300000
          });
        }
      },
      error: (err) => {
        this.paymentError = err.error.message;
        this._tosterService.error("Your Order Id: " + this.orderId, 'Payment failed', {
          timeOut: 300000
        });
        this.hideLoader()
      },
      complete: () => {
        this.ngOnInit();
        if (this.isPaymentSuccess)
          this._tosterService.success("Your Payment Id: " + this.paymentId, "Payment Success", {
            timeOut: 300000
          });
        this.hideLoader()
      }
    });
  }

  showLoader() {
    this.isShowLoader = true;
  }
  hideLoader() {
    this.isShowLoader = false;
  }

  ngOnDestroy(): void {
    this.orderId = "";
    this.paymentError = "";
    this.paymentId = "";
    this.isPaymentSuccess = false;
  }
}
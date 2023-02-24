import { Component, Input, OnInit } from '@angular/core';
import { NavigationCancel, Event, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css']
})
export class LoaderComponent implements OnInit {

  @Input() showLoader: Boolean = true;

  constructor(private router: Router) {
    this.router.events.subscribe((routerEvent: Event) => {

      if (routerEvent instanceof NavigationStart) {
        this.showLoader = true;
      }

      if (routerEvent instanceof NavigationEnd ||
        routerEvent instanceof NavigationError ||
        routerEvent instanceof NavigationCancel) {
        this.showLoader = false;
      }
    });
  }

  ngOnInit(): void { }

}

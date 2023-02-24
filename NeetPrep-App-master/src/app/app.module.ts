import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoaderComponent } from './shared/loader/loader.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { SignupComponent } from './components/signup/signup.component';
import { ContactComponent } from './components/contact/contact.component';
import { NavBarComponent } from './shared/components/nav-bar/nav-bar.component';
import { ProfileComponent } from './components/profile/profile.component';
import { FooterComponent } from './shared/components/footer/footer.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { HttpInterceptorInterceptor } from './shared/interceptor/http-interceptor.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import {TableModule} from 'primeng/table';
import { CustomTableComponent } from './components/custom-table/custom-table.component';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import {ProgressBarModule} from 'primeng/progressbar';
import {SliderModule} from 'primeng/slider';
import {MultiSelectModule} from 'primeng/multiselect';
import {DialogModule} from 'primeng/dialog';
import {ContextMenuModule} from 'primeng/contextmenu';

@NgModule({
  declarations: [
    AppComponent,
    LoaderComponent,
    HomeComponent,
    LoginComponent,
    SignupComponent,
    ContactComponent,
    NavBarComponent,
    ProfileComponent,
    FooterComponent,
    DashboardComponent,
    CustomTableComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    TableModule,
    DropdownModule,
    FormsModule,
		SliderModule,
		DialogModule,
		MultiSelectModule,
		ContextMenuModule,
		ButtonModule,
		ToastrModule,
    InputTextModule,
    ProgressBarModule,
    ToastrModule.forRoot({
      timeOut: 10000,
      closeButton: true,
      progressBar: true
    })
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpInterceptorInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

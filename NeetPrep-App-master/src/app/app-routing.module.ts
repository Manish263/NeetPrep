import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContactComponent } from './components/contact/contact.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { ProfileComponent } from './components/profile/profile.component';
import { SignupComponent } from './components/signup/signup.component';
import { AuthenticationGuard } from './shared/guards/authentication.guard';

const routes: Routes = [
  {
    path : "",
    component : HomeComponent
  },
  {
    path : "home",
    component : HomeComponent
  },
  {
    path : "contact",
    component : ContactComponent
  },
  {
    path : "login",
    component : LoginComponent
  },
  {
    path : "signup",
    component : SignupComponent
  },
  {
    path : "profile",
    component : ProfileComponent,
    canActivate : [AuthenticationGuard]
  },
  {
    path : "dashboard",
    component : DashboardComponent,
    canActivate : [AuthenticationGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

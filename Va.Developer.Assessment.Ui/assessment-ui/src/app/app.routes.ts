import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './static/home/home.component';
import { AboutComponent } from './static/about/about.component';
import { ContactComponent } from './static/contact/contact.component';
export const routes: Routes = [
  {path: 'home', component: HomeComponent, title: 'Moshele Seoka Assessment | Home'},
  {path: 'about', component: AboutComponent, title: 'Moshele Seoka Assessment | About'},
  {path: 'contact', component: ContactComponent, title: 'Moshele Seoka Assessment | Contact'},
  {path: 'dashboard', loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule)}
];
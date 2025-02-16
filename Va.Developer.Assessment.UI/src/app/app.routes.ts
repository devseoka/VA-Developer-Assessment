import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'home',
    loadComponent: () => import('./static/home/home.component').then(c => c.HomeComponent),
    title: 'Va Developer Assessment | Seoka Moshele | Home'
  },
  {
    path: 'about',
    loadComponent: () => import('./static/about/about.component').then(c => c.AboutComponent),
    title: 'Va Developer Assessment | Seoka Moshele | About'
  },
  {
    path: 'contact',
    loadComponent: () => import('./static/contact/contact.component').then(c => c.ContactComponent),
    title: 'Va Developer Assessment | Seoka Moshele | Contact'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

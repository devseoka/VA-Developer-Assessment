import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

const routes: Routes = [
  {
    path: 'accounts',
    loadComponent: () => import('./accounts/accounts.component').then(c => c.AccountsComponent),
    title: 'Va Developer Assessment | Seoka Moshele | Accounts'
  },
  {
    path: 'persons',
    loadComponent: () => import('./persons/persons.component').then(c => c.PersonsComponent),
    title: 'Va Developer Assessment | Seoka Moshele | About'
  },
  {
    path: 'transactions',
    loadComponent: () => import('./transactions/transactions.component').then(c => c.TransactionsComponent),
    title: 'Va Developer Assessment | Seoka Moshele | Contact'
  },
  {path: '', redirectTo: 'persons', pathMatch: 'full'},
  {path: '**', redirectTo: 'persons', pathMatch: 'prefix'}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRouting { }
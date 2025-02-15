import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { PersonsComponent } from "./persons/persons.component";
import { TransactionsComponent } from "./transactions/transactions.component";
import { AccountsComponent } from "./accounts/accounts.component";

const routes: Routes = [
  {
    path: 'persons',
    title: 'Moshele Seoka | Person Details',
    loadComponent: () => import('./persons/persons.component').then(c => c.PersonsComponent)
  },
  {
    path: 'accounts',
    loadComponent: () => import('./accounts/accounts.component').then(c => c.AccountsComponent),
    title: 'Moshele Seoka | Account Details'
  },
  {
    path: 'transactions',
    loadComponent: () => import('./transactions/transactions.component').then(c => c.TransactionsComponent),
    title: 'Moshele Seoka | Transactions Details'
  },
  { path: '', redirectTo: 'persons', pathMatch: 'full' },
  { path: '**', redirectTo: 'persons', pathMatch: 'full' }
]
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
import { NgModule } from "@angular/core";
import { DashboardRoutingModule } from "./dashboard-routing.module";
import { AccountsComponent } from "./accounts/accounts.component";
import { PersonsComponent } from "./persons/persons.component";
import { TransactionsComponent } from "./transactions/transactions.component";
import { RouterModule } from "@angular/router";

@NgModule({
    imports: [
        DashboardRoutingModule,
        RouterModule,
        PersonsComponent,
        TransactionsComponent,
        AccountsComponent]
})
export class DashboardModule { }
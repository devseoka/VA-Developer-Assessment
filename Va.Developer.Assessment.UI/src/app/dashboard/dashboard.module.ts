import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { SharedModule } from "../shared/shared.module";
import { DashboardRouting } from "./dashboard.routes";
import { PersonsComponent } from "./persons/persons.component";
import { AccountsComponent } from "./accounts/accounts.component";
import { TransactionsComponent } from "./transactions/transactions.component";

@NgModule({
    declarations: [],
    imports: [ReactiveFormsModule, CommonModule, SharedModule, DashboardRouting,
        PersonsComponent,
        AccountsComponent,
        TransactionsComponent
    ],
})
export class DashboardModule{}
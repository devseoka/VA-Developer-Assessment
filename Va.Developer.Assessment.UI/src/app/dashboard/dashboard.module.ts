import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { SharedModule } from "../shared/shared.module";

@NgModule({
    declarations: [],
    imports: [ReactiveFormsModule, CommonModule, SharedModule],
})
export class DashboardModule{}
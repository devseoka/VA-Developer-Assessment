import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { User } from "src/app/models/person.model";
import { PersonService } from "src/app/services/person.service";

@Component({
    selector: 'app-persons',
    templateUrl: './persons.component.html',
    standalone: true,
    imports: [HttpClientModule, CommonModule],
    providers: [PersonService]
})
export class PersonsComponent implements OnInit {
    users: User[] = []
    constructor(private readonly personService: PersonService) { }

    ngOnInit(): void {
        this.personService.get(1, 10).subscribe({
            next: (response) => {
                this.users = response.data
            }<mat-accordion displayMode="default" [multi]="true"
                           [hideToggle]="false">
                <mat-expansion-panel [hideToggle]="false">
                    <mat-expansion-panel-header>Panel header</mat-expansion-panel-header>
                     Panel body
                    <mat-action-row>
                        <button mat-button (click)="onAction(event)">Action1</button>
                    </mat-action-row>
                </mat-expansion-panel>
            </mat-accordion>
        })
    }

}
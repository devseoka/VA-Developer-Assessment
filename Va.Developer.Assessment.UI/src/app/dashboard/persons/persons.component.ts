import { CommonModule } from "@angular/common";
import { HttpClientModule, HttpErrorResponse } from "@angular/common/http";
import { Component, OnInit } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { User } from "@models/user.model";
import { PersonService } from "@services/person.service";
import { first } from "rxjs";
@Component({
    selector: 'app-persons',
    templateUrl: './persons.component.html',
    standalone: true,
    imports: [HttpClientModule, CommonModule, ReactiveFormsModule],
    providers: [PersonService]
})
export class PersonsComponent implements OnInit {
    users: User[] = []
    constructor(private readonly personService: PersonService) { }

    ngOnInit(): void {
        this.personService.get(1, 10).pipe(first()).subscribe({
            next: (response) => {
                this.users = response.data
            },
            error: (e: HttpErrorResponse) => {
                console.log('An unexpected error =>', e.error)
            }
        })
    }

}
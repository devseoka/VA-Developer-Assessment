import { CommonModule } from "@angular/common";
import { HttpClientModule, HttpErrorResponse } from "@angular/common/http";
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
            },
            error: (e: HttpErrorResponse) => {
                console.log('An unexpected error =>', e.error)
            }
        })
    }

}
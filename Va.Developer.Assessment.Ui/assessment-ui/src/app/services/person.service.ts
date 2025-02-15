import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Response } from "../models/res/response";
import { User } from "../models/person.model";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn: 'root'
})
export class PersonService {
    private endpoint = `${environment.api}/persons`

    constructor(private http: HttpClient){}
    get(page:number, size: number){
        const uri = `${this.endpoint}?pageNumber=${page}&pageSize=${size}`
        return this.http.get<Response<User[]>>(uri)
    }
}
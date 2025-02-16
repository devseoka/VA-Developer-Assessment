import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { Response } from "@models/response.model";
import { User } from "@models/user.model";

@Injectable({
    providedIn: 'root'
})
export class PersonService {
    private endpoint = `${environment.api}/persons`
    constructor(private readonly http: HttpClient){}
    get(page: number, size: number){
        const uri = `${this.endpoint}?pageNumber=${page}&pageSize=${size}`
        return this.http.get<Response<User[]>>(uri)
    }
}
import { Account } from "./account.model";

export interface User {
    id: number;
    firstName: string;
    lastName: string;
    idNo: string;
    accounts: Account[];
}
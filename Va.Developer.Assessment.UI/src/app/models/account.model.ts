import { Transaction } from "./transaction.model";


export interface Account {
    userId: number;
    accountNo: string;
    balance: number;
    transactions: Transaction[];
    id: number;
}

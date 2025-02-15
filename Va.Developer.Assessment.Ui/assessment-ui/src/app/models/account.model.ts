import { Transaction } from "./transaction.model";

export interface Account {
    id: number;
    userId: number;
    accountNo: string;
    balance: number;
    transactions: Transaction[];
}

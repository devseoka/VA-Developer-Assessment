import type { Account}  from './account.model';
export interface User {
  firstName: string
  lastName: string
  idNo: string
  accounts: Account[]
  id: string
}

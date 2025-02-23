import { helpers, required, numeric, minLength } from '@vuelidate/validators';

const notFutureDate = (value: Date) => value <= new Date();
const notZero = (value: number) => value !== 0;

export const transactionRules = {
  accountNo: {
    required,
  },
  balance: {

  },
  orderedDate: {
    required,
    notFutureDate: helpers.withMessage('Date cannot be in the future', notFutureDate),
  },
  total: {
    required,
    notZero: helpers.withMessage('Total cannot be zero', notZero),
    numeric,
  },
  description: {
    required,
  },
};

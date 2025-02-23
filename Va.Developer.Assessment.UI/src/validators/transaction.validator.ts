import { helpers, required, numeric, minLength } from '@vuelidate/validators';

const notFutureDate = (value: string) => {
  const current = new Date()
  const currentDate = new Date(current.toDateString())

  const transactionDate = new Date(value)
  const normalizedTransactionDate = new Date(transactionDate.toDateString())

  return normalizedTransactionDate <= currentDate;
};

const notZero = (value: number) => value !== 0;
const isNumeric = (value: number) => !isNaN(value);

export const transactionRules = {
  accountNo: {
    required,
  },
  orderedDate: {
    required,
    notFutureDate: helpers.withMessage('Date cannot be in the future', notFutureDate),
  },
  total: {
    required,
    notZero: helpers.withMessage('Total cannot be zero', notZero),
    isNumeric: helpers.withMessage('Transaction amount must be a numeric', isNumeric),
  },
  description: {
    required,
  },
};

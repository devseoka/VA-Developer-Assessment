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
  accountId: {
    required,
  },
  orderedDate: {
    required: helpers.withMessage('Transaction date is required', required),
    notFutureDate: helpers.withMessage('Transaction date cannot be in the future', notFutureDate),
  },
  total: {
    required: helpers.withMessage('Transaction amount cannot be zero', notZero),
    notZero: helpers.withMessage('Transaction amount cannot be zero', notZero),
    isNumeric: helpers.withMessage('Transaction amount must be a numeric', isNumeric),
  },
  description: {
    required: helpers.withMessage('Transaction description is required', notZero),
  },
};

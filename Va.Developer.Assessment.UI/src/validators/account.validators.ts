import { required, minLength, helpers, numeric, maxLength } from "@vuelidate/validators";

const isAccountRequired = (value: string) => {
  console.log("Value received in isAccountRequired:", value);
  return typeof value === "string" && value.trim().length > 0;
}
const isAccountNumeric = (value: string) => {
  const isNumeric = !isNaN(Number(value));
  return isNumeric
}
export const rules = {
  accountNo: {
    isAccountRequired: helpers.withMessage('Account number is required', isAccountRequired),
    minLength: helpers.withMessage('Account number should have at least 5 digits', minLength(5)),
    maxLength: helpers.withMessage('Account number should not have more than 12 digits', maxLength(12)),
    isAccountNumeric: helpers.withMessage('Account number must be numbers only', isAccountNumeric)
  },
  balance: {
    required: helpers.withMessage('Account Balance is required', required),
    minLength: helpers.withMessage('Account Balance should have at least 1 digit', minLength(1)),
    numeric: helpers.withMessage('Account Balance must be numbers only', numeric)
  },
}

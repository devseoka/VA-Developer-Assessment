import { required, minLength } from "@vuelidate/validators";

export const rules = {
  accountNo: {
     required: {},
     minLength: minLength(2),
  },
  balance: {
     required: {},
     minLength: minLength(2),
     numeric: {}
  },
}

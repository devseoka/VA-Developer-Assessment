import { maxLength, minLength, numeric } from "@vuelidate/validators";

export const rules = {
  firstName: {
     required: {},
     minLength: minLength(2),
  },
  lastName: {
     required: {},
     minLength: minLength(2),
     numeric: {}
  },
  idNo: {
     required: {},
      minLength: minLength(13),
      maxLength: maxLength(13),
      numeric,
  }
}

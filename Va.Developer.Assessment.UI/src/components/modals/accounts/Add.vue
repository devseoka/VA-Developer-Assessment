<template>
  <div id="add-account" tabindex="-1" aria-hidden="true"
    class="hidden overflow-y-auto overflow-x-hidden fixed top-0 right-0 left-0 z-50 justify-center items-center w-full md:inset-0 h-[calc(100%-1rem)] max-h-full">
    <div class="relative p-4 w-full max-w-md max-h-full">
      <div class="relative bg-white rounded-lg shadow-sm">
        <div class="flex items-center justify-between p-4 md:p-5 border-b rounded-t border-assessment-secondary-200">
          <h3 class="text-xl font-semibold text-assessment-secondary-900">
            Add a debit or credit account
          </h3>
          <button type="button"
            class="end-2.5 text-assessment-secondary-400 bg-transparent hover:bg-assessment-secondary-200 hover:text-assessment-secondary-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center"
            data-modal-hide="add-account">
            <svg class="w-3 h-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
              <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6" />
            </svg>
            <span class="sr-only">Close modal</span>
          </button>
        </div>
        <div class="p-4 md:p-5">
          <form class="space-y-4" @submit.prevent="onAddAccount">
            <div class="col-span-2">
              <label for="accountNo" class="block mb-2 text-sm font-medium text-assessment-secondary-900">Account
                Number</label>
              <input type="text" id="accountNo" v-model="accountForm.accountNo" @blur="account$.accountNo.$touch()"
                class="bg-assessment-secondary-50 border border-assessment-secondary-300 text-assessment-secondary-900 text-sm rounded-lg focus:ring-assessment-accent-500 focus:border-assessment-accent-500 block w-full p-2.5"
                placeholder="Account number" />
              <template v-if="account$.accountNo.$error">
                <span v-for="(error, i) in account$.accountNo.$errors" :key="i"
                  class="block text-sm text-assessment-primary-500 font-medium">
                  {{ error.$message }}
                </span>
              </template>
            </div>
            <button type="submit"
              class="w-full text-white bg-assessment-accent-700 hover:bg-assessment-accent-800 focus:ring-4 focus:outline-none focus:ring-assessment-accent-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center">
              Add account
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import type { Account } from '@/models/account.model';
import type { Response } from '@/models/response.model';
import { rules } from '@/validators/account.validators';
import useVuelidate from '@vuelidate/core';
import axios, { AxiosError } from 'axios';
import { onMounted, reactive, ref } from 'vue';

const messages = ref<string[]>([])
const succeeded = ref<boolean>(false)

const initAccountForm = () => {
  return reactive<Account>({
    accountNo: '',
    balance: 0,
    userId: props.userId,
    id: 0,
    transactions: []
  })
}


const props = defineProps({
  userId: {
    required: true,
    type: Number
  }
})
const onAccountAdded = defineEmits<{ (e: 'account-added-event', response: Response<Account>): void }>()

const accountForm = reactive<Account>({
  accountNo: '',
  balance: 1,
  userId: props.userId,
  id: 0,
  transactions: []
})
const account$ = useVuelidate(rules, accountForm)

const onAddAccount = async () => {
  account$.value.$touch()
  if (account$.value.$invalid) {
    return
  }
  try {
    var response = (await axios.post<Response<Account>>('https://localhost:7297/api/accounts', accountForm))
    var result = response.data
    messages.value = [result.message]
    succeeded.value = result.succeeded;
    onAccountAdded('account-added-event', result)
  }
  catch (e) {
    if (e instanceof AxiosError && typeof e.response !== 'undefined') {
      var err = (e.response?.data.errors as string[])
      succeeded.value = false;
      if (typeof err !== 'undefined') {
        messages.value = err.length > 0 ? err : e.response?.data.errors
      }
      else {
        const errors = ['An unexpected error occured. If the issue persist contact support team im@seokamoshele.digital'];
        messages.value = errors
      }
    }
  }
}
</script>

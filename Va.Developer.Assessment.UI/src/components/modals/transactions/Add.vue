<template>
  <div id="add-transaction" tabindex="-1" aria-hidden="true"
    class="hidden overflow-y-auto overflow-x-hidden fixed top-0 right-0 left-0 z-50 justify-center items-center w-full md:inset-0 h-[calc(100%-1rem)] max-h-full">
    <div class="relative p-4 w-full max-w-md max-h-full">
      <div class="relative bg-white rounded-lg shadow-sm">
        <div class="flex items-center justify-between p-4 md:p-5 border-b rounded-t border-assessment-secondary-200">
          <h3 class="text-xl font-semibold text-assessment-secondary-900">
            Add a debit or credit transaction
          </h3>
          <button type="button"
            class="end-2.5 text-assessment-secondary-400 bg-transparent hover:bg-assessment-secondary-200 hover:text-assessment-secondary-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center"
            data-modal-hide="add-transaction">
            <svg class="w-3 h-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
              <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6" />
            </svg>
            <span class="sr-only">Close modal</span>
          </button>
        </div>
        <div class="p-4 md:p-5">
          <form class="space-y-4" @submit.prevent="onAddTransaction">
            <div>
              <label for="orderedDate" class="block mb-2 text-sm font-medium text-assessment-secondary-900">Transaction
                Date</label>
              <input type="datetime-local" id="orderedDate" @input="transaction$.orderedDate.$touch()"
                class="bg-assessment-secondary-50 border border-assessment-secondary-300 text-assessment-secondary-900 text-sm rounded-lg focus:ring-assessment-accent-500 focus:border-assessment-accent-500 block w-full p-2.5"
                placeholder="dd/mm/yyyy" v-model="transactionForm.orderedDate" />
              <template v-if="transaction$.orderedDate.$error">
                <span v-for="(error, i) in transaction$.orderedDate.$errors" :key="i"
                  class="block text-sm text-assessment-primary-500 font-medium">
                  {{ error.$message }}
                </span>
              </template>
            </div>
            <div>
              <label for="amount" class="block mb-2 text-sm font-medium text-assessment-secondary-900">Your Transaction
                Amount</label>
              <input type="number" id="amount" placeholder="20" @input="transaction$.total.$touch()"
                v-model="transactionForm.total"
                class="bg-assessment-secondary-50 border border-assessment-secondary-300 text-assessment-secondary-900 text-sm rounded-lg focus:ring-assessment-accent-500 focus:border-assessment-accent-500 block w-full p-2.5" />
              <template v-if="transaction$.total.$error">
                <span v-for="(error, i) in transaction$.total.$errors" :key="i"
                  class="block text-sm text-assessment-primary-500 font-medium">
                  {{ error.$message }}
                </span>
              </template>
            </div>
            <div class="col-span-2">
              <label for="description" class="block mb-2 text-sm font-medium text-assessment-secondary-900">Product
                Description</label>
              <textarea id="description" rows="4" @input="transaction$.description.$touch()"
                v-model="transactionForm.description"
                class="block p-2.5 w-full text-sm text-assessment-secondary-900 bg-assessment-secondary-50 rounded-lg border border-assessment-secondary-300 focus:ring-assessment-accent-500 focus:border-assessment-accent-500"
                placeholder="Write product description here"></textarea>
            </div>
            <button type="submit"
              class="w-full text-white bg-assessment-accent-700 hover:bg-assessment-accent-800 focus:ring-4 focus:outline-none focus:ring-assessment-accent-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center">
              Add Transaction
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
import type { Transaction } from '@/models/transaction.model';
import type { Response } from '@/models/response.model';
import { transactionRules } from '@/validators/transaction.validator';
import useVuelidate from '@vuelidate/core';
import axios, { AxiosError } from 'axios';
import { Modal, type ModalInterface } from 'flowbite';
import { onMounted, reactive, ref, type PropType } from 'vue';

const succeeded = ref<boolean>(false)
const messages = ref<string[]>([])
const message = ref<string>('')

const props = defineProps({
   accountId: {
    type: Number,
    required: true
   },
   modal: {
    type: Object as PropType<ModalInterface>,
    required: true
   }
})

const transactionForm = reactive<Transaction>({
  accountId: props.accountId,
  description: '',
  orderedDate: new Date(),
  total: 0,
  id: 0
})

const transaction$ = useVuelidate(transactionRules, transactionForm)

onMounted(() => {
  const $modalEl = document.getElementById('add-transaction')
  if ($modalEl) {
  }
})

const onAddTransaction = async () => {
  transaction$.value.$touch()
  if (transaction$.value.$invalid) {
    return
  }
  try {

    transactionForm.accountId = props.accountId
    const uri = `https://localhost:7297/api/Transactions`
    props.modal.hide();
    var response = await axios.post<Response<Transaction>>(uri, transactionForm)
    var result = response.data
    succeeded.value = true
    message.value = result.message
    props.modal.hide();
  } catch (e) {
    if (e instanceof AxiosError && typeof e.response !== 'undefined') {
      var err = e.response.data.errors as string[]
      succeeded.value = false
      messages.value = err.length > 0 ? err : e.response?.data.errors
    } else {
      message.value = 'An unexpected error whil adding a transaction'
    }
    props.modal.hide();
  }
}

</script>

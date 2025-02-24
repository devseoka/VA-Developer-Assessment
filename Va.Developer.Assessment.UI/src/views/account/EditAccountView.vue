<template>
  <Toaster v-if="message" :message="message" :duration="5000" :type="modalType" />
  <TableHeader v-if="editForm.accountNo" :subtitle="`Balance: ${balance}`" :title="`Account: ${editForm.accountNo}`" />
  <AlertError :succeeded="false" :messages="messages" />
  <div class="max-w-2xl px-4 py-8 mx-auto lg:py-16">
    <h2 class="mb-4 text-xl font-bold text-assessment-secondary-500">Update Account</h2>
    <form @submit.prevent="">
      <div class="grid gap-4 mb-4 sm:grid-cols-2 sm:gap-6 sm:mb-5">
        <div class="w-full">
          <label for="accountNo" class="block mb-2 text-sm font-medium text-assessment-secondary-900">Account
            Number</label>
          <input type="text" v-model="editForm.accountNo"
            class="bg-assessment-secondary-50 border border-assessment-secondary-300 text-assessment-secondary-900 text-sm rounded-lg focus:ring-assessment-accent-600 focus:border-assessment-accent-600 block w-full p-2.5"
            placeholder="Product accountNo" />
        </div>
        <div class="w-full">
          <label for="price" class="block mb-2 text-sm font-medium text-assessment-secondary-900">Balance</label>
          <input type="text" id="disabled-input" aria-label="disabled input"
            class="mb-5 bg-assessment-secondary-100 border border-assessment-secondary-300 text-assessment-secondary-900 text-sm rounded-lg focus:ring-assessment-accent-500 focus:border-assessment-accent-500 block w-full p-2.5 cursor-not-allowed"
            v-model="editForm.balance" disabled />
        </div>
      </div>
      <div class="flex items-center space-x-4">
        <button type="submit"
          class="text-white bg-assessment-accent-700 hover:bg-assessment-accent-800 focus:ring-4 focus:outline-none focus:ring-assessment-accent-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center">
          Update Account
        </button>
        <button type="button" :disabled="v$.$invalid"
          class="text-assessment-assessment-accent-600 inline-flex items-center hover:text-white border border-assessment-assessment-accent-600 hover:bg-assessment-assessment-accent-600 focus:ring-4 focus:outline-none focus:ring-assessment-assessment-accent-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center">
          <svg class="w-5 h-5 mr-1 -ml-1" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
            <path fill-rule="evenodd"
              d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z"
              clip-rule="evenodd"></path>
          </svg>
          Delete
        </button>
      </div>
    </form>
  </div>
  <div class="border-y-2 border-assessment-secondary-400 flex flex-col">
    <div class="flex w-full justify-between items-center">
      <TableHeader :subtitle="'Manage all transaction related to this account number.'"
        :title="`${editForm.accountNo} Transactions`" />
      <button type="button" data-modal-target="add-transaction" data-modal-toggle="add-transaction"
        class="py-3 px-2 bg-assessment-accent-500 rounded-lg text-assessment-accent-50 cursor-pointer hover:bg-assessment-accent-600 focus:ring-4 focus:ring-assessment-accent-300 focus:bg-assessment-accent-700 font-semibold text-sm mx-3 focus:outline-none">
        Add Transaction
      </button>
    </div>
    <div class="relative overflow-x-auto flex-col py-4">
      <table class="w-full text-sm text-left rtl:text-right text-assessment-secondary-500">
        <thead class="text-xs text-assessment-secondary-700 uppercase bg-assessment-secondary-100">
          <tr>
            <th scope="col" class="px-6 py-3">Description</th>
            <th scope="col" class="px-6 py-3">Transaction Date</th>
            <th scope="col" class="px-6 py-3">Price</th>
            <th scope="col" class="px-6 py-3">Action</th>
          </tr>
        </thead>
        <tbody>
          <tr class="bg-white" v-for="transaction in editForm.transactions" :key="transaction.id">
            <th scope="row" class="px-6 py-4 font-medium text-assessment-secondary-900 whitespace-nowrap">
              {{ transaction.description }}
            </th>
            <td class="px-6 py-4">
              {{ transaction.orderedDate }}
            </td>
            <td class="px-6 py-4">
              {{ useCurrency(transaction.total).formattedPrice }}
            </td>
            <td class="px-6 py-4">
              <a :href="`/transactions/${transaction.id}`"
                class="font-medium text-assessment-accent-600 hover:underline">Edit</a>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <Add :modal="modal!" :account-id="editForm.id" @transaction-added="onTransactionAdd"/>
  </div>
</template>

<script setup lang="ts">
import type { Response } from '@/models/response.model'
import axios, { AxiosError } from 'axios'
import { computed, onMounted, reactive, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import type { Account } from '@/models/account.model'

import TableHeader from '@/components/table/TableHeader.vue'
import Toaster from '@/components/shared/Toaster.vue'
import AlertError from '@/components/shared/AlertError.vue'
import Add from '@/components/modals/transactions/Add.vue'

import { useCurrency } from '@/extensions/currency.pipe'
import useVuelidate from '@vuelidate/core'
import { rules } from '@/validators/account.validators'
import { Modal, type ModalInterface } from 'flowbite'
import type { Transaction } from '@/models/transaction.model'

const route = useRoute()
const router = useRouter()

const succeeded = ref<boolean>(false)
const messages = ref<string[]>([])
const message = ref<string>('')
const modalType = ref<string>('')


const editForm = reactive<Account>({
  id: 0,
  accountNo: '',
  balance: 0,
  userId: 0,
  transactions: [],
})

const v$ = useVuelidate(rules, editForm)

const balance = computed(() => {
  return useCurrency(editForm.balance).formattedPrice.value.toString()
})

let modal = ref<ModalInterface | null>(null);

onMounted(() => {
  get()
  const $modalEl = document.getElementById('add-transaction')
  if($modalEl){
    modal.value = new Modal($modalEl, {
       backdrop: 'dynamic',
       closable: true,
       placement: 'center',
       backdropClasses:
       'bg-gray-900/50 fixed inset-0 z-40',
       onHide() {
          get()
       },
    })
  }
})

const get = async () => {
  try {
    const response = await axios.get<Response<Account>>(
      `https://localhost:7297/api/accounts/${route.params.id}`,
    )
    const result = response.data
    Object.assign(editForm, result.data)
  } catch (error) {
    console.error('Failed to fetch account data:', error)
    router.push('/users')
  }
}

const onUpdate = () => {
  v$.value.$touch()
  if (v$.value.$invalid) {
    return
  }
}
const onTransactionAdd = (response: Response<Transaction>) => {
  editForm.transactions.push(response.data)
  message.value = response.message
  modalType.value = response.succeeded ? 'success' : 'error'
  succeeded.value = response.succeeded
}
</script>

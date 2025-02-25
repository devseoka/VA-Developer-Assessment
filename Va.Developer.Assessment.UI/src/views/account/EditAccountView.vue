<template>
  <Toaster v-if="modalType" :type="modalType" :message="message" :duration="5000" />
  <TableHeader
    v-if="editForm.accountNo"
    :subtitle="`Balance: ${balance}`"
    :title="`Account: ${editForm.accountNo}`"
  />
  <AlertError :succeeded="false" :messages="messages" />
  <div class="max-w-2xl px-4 py-8 mx-auto lg:py-16">
    <h2 class="mb-4 text-xl font-bold text-assessment-secondary-500">Update Account</h2>
    <form @submit.prevent="">
      <div class="grid gap-4 mb-4 sm:grid-cols-2 sm:gap-6 sm:mb-5">
        <div class="w-full">
          <label
            for="accountNo"
            class="block mb-2 text-sm font-medium text-assessment-secondary-900"
            >Account Number</label
          >
          <input
            type="text"
            v-model="editForm.accountNo"
            @blur="v$.accountNo.$touch()"
            class="bg-assessment-secondary-50 border border-assessment-secondary-300 text-assessment-secondary-900 text-sm rounded-lg focus:ring-assessment-accent-600 focus:border-assessment-accent-600 block w-full p-2.5"
            placeholder="account Number"
          />
        </div>
        <div class="w-full">
          <label for="price" class="block mb-2 text-sm font-medium text-assessment-secondary-900"
            >Balance</label
          >
          <input
            type="text"
            id="disabled-input"
            aria-label="disabled input"
            class="mb-5 bg-assessment-secondary-100 border border-assessment-secondary-300 text-assessment-secondary-900 text-sm rounded-lg focus:ring-assessment-accent-500 focus:border-assessment-accent-500 block w-full p-2.5 cursor-not-allowed"
            v-model="editForm.balance"
            disabled
          />
        </div>
      </div>
      <div class="flex items-center space-x-4">
        <button
          type="submit"
          class="w-full text-white bg-assessment-accent-700 hover:bg-assessment-accent-800 focus:ring-4 focus:outline-none focus:ring-assessment-accent-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center"
        >
          Update Account
        </button>
      </div>
    </form>
  </div>
  <div class="border-y-2 border-assessment-secondary-400 flex flex-col">
    <div class="flex w-full justify-between items-center">
      <TableHeader
        :subtitle="'Manage all transaction related to this account number.'"
        :title="`${editForm.accountNo} Transactions`"
      />
      <button
        type="button"
        data-modal-target="add-transaction"
        data-modal-toggle="add-transaction"
        class="py-3 px-2 bg-assessment-accent-500 rounded-lg text-assessment-accent-50 cursor-pointer hover:bg-assessment-accent-600 focus:ring-4 focus:ring-assessment-accent-300 focus:bg-assessment-accent-700 font-semibold text-sm mx-3 focus:outline-none"
      >
        Add Transaction
      </button>
    </div>
    <div class="relative overflow-x-auto flex-col py-4">
      <table
        class="w-full text-sm text-left rtl:text-right text-assessment-secondary-500"
        v-if="transactions().length"
      >
        <thead class="text-xs text-assessment-secondary-700 uppercase bg-assessment-secondary-100">
          <tr>
            <th scope="col" class="px-6 py-3">Description</th>
            <th scope="col" class="px-6 py-3">Transaction Date</th>
            <th scope="col" class="px-6 py-3">Price</th>
            <th scope="col" class="px-6 py-3">Action</th>
          </tr>
        </thead>
        <tbody>
          <tr class="bg-white" v-for="transaction in transactions()" :key="transaction.id">
            <th
              scope="row"
              class="px-6 py-4 font-medium text-assessment-secondary-900 whitespace-nowrap"
            >
              {{ transaction.description }}
            </th>
            <td class="px-6 py-4">
              {{ transaction.orderedDate }}
            </td>
            <td class="px-6 py-4">
              {{ useCurrency(transaction.total).formattedPrice }}
            </td>
            <td class="px-6 py-4">
              <a
                :href="`/transactions/${transaction.id}`"
                class="font-medium text-assessment-accent-600 hover:underline"
                >Edit</a
              >
            </td>
          </tr>
        </tbody>
      </table>
      <Pagination
        v-if="editForm.transactions.length > 0"
        :total="editForm.transactions.length"
        :itemsPerPage="itemsPerPage"
        v-model:currentPage="currentPage"
      />
    </div>
    <Add :account-id="editForm.id" @transaction-added="onTransactionAdd" />
  </div>
</template>

<script setup lang="ts">
import type { Response } from '@/models/response.model'
import axios from 'axios'
import { computed, onMounted, reactive, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import type { Account } from '@/models/account.model'

import TableHeader from '@/components/table/TableHeader.vue'
import Pagination from '@/components/table/Pagination.vue'
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

const itemsPerPage = ref<number>(5)
const currentPage = ref<number>(1)

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

const transactions = () => {
  const orderedTransactions = [...editForm.transactions].sort((a, b) => {
    return new Date(b.processedDate).getTime() - new Date(a.processedDate).getTime()
  })
  const start = (currentPage.value - 1) * itemsPerPage.value
  const end = start + itemsPerPage.value
  return orderedTransactions.slice(start, end)
}

onMounted(() => {
  get()
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
  const $modalEl = document.getElementById('add-transaction')
  if ($modalEl) {
    editForm.transactions.push(response.data)
    message.value = response.message
    modalType.value = response.succeeded ? 'success' : 'error'
    succeeded.value = response.succeeded
  }
}
</script>

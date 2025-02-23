<template>
  <Toaster v-if="message" :message="message" :duration="3000" :type="modalType" />
  <TableHeader
    v-if="editForm.accountNo"
    :subtitle="`Balance: ${balance}`"
    :title="`Account: ${editForm.accountNo}`"
  />
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
            class="bg-assessment-secondary-50 border border-assessment-secondary-300 text-assessment-secondary-900 text-sm rounded-lg focus:ring-assessment-accent-600 focus:border-assessment-accent-600 block w-full p-2.5"
            placeholder="Product accountNo"
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
          class="text-white bg-assessment-accent-700 hover:bg-assessment-accent-800 focus:ring-4 focus:outline-none focus:ring-assessment-accent-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center"
        >
          Update Account
        </button>
        <button
          type="button"
          :disabled="v$.$invalid"
          class="text-assessment-assessment-accent-600 inline-flex items-center hover:text-white border border-assessment-assessment-accent-600 hover:bg-assessment-assessment-accent-600 focus:ring-4 focus:outline-none focus:ring-assessment-assessment-accent-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center"
        >
          <svg
            class="w-5 h-5 mr-1 -ml-1"
            fill="currentColor"
            viewBox="0 0 20 20"
            xmlns="http://www.w3.org/2000/svg"
          >
            <path
              fill-rule="evenodd"
              d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z"
              clip-rule="evenodd"
            ></path>
          </svg>
          Delete
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
    </div>
    <div
      id="add-transaction"
      tabindex="-1"
      aria-hidden="true"
      class="hidden overflow-y-auto overflow-x-hidden fixed top-0 right-0 left-0 z-50 justify-center items-center w-full md:inset-0 h-[calc(100%-1rem)] max-h-full"
    >
      <div class="relative p-4 w-full max-w-md max-h-full">
        <div class="relative bg-white rounded-lg shadow-sm">
          <div
            class="flex items-center justify-between p-4 md:p-5 border-b rounded-t border-assessment-secondary-200"
          >
            <h3 class="text-xl font-semibold text-assessment-secondary-900">
              Add a debit or credit transaction
            </h3>
            <button
              type="button"
              class="end-2.5 text-assessment-secondary-400 bg-transparent hover:bg-assessment-secondary-200 hover:text-assessment-secondary-900 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-center"
              data-modal-hide="add-transaction"
            >
              <svg
                class="w-3 h-3"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 14 14"
              >
                <path
                  stroke="currentColor"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
                />
              </svg>
              <span class="sr-only">Close modal</span>
            </button>
          </div>
          <div class="p-4 md:p-5">
            <form class="space-y-4" @submit.prevent="onAddTransaction">
              <div>
                <label
                  for="orderedDate"
                  class="block mb-2 text-sm font-medium text-assessment-secondary-900"
                  >Transaction Date</label
                >
                <input
                  type="datetime-local"
                  id="orderedDate"
                  @input="transaction$.orderedDate.$touch()"
                  class="bg-assessment-secondary-50 border border-assessment-secondary-300 text-assessment-secondary-900 text-sm rounded-lg focus:ring-assessment-accent-500 focus:border-assessment-accent-500 block w-full p-2.5"
                  placeholder="dd/mm/yyyy"
                  v-model="transactionForm.orderedDate"
                />
                <template v-if="transaction$.orderedDate.$error">
                  <span
                    v-for="(error, i) in transaction$.orderedDate.$errors"
                    :key="i"
                    class="block text-sm text-assessment-primary-500 font-medium"
                  >
                    {{ error.$message }}
                  </span>
                </template>
              </div>
              <div>
                <label
                  for="amount"
                  class="block mb-2 text-sm font-medium text-assessment-secondary-900"
                  >Your Transaction Amount</label
                >
                <input
                  type="number"
                  id="amount"
                  placeholder="20"
                  @input="transaction$.total.$touch()"
                  v-model="transactionForm.total"
                  class="bg-assessment-secondary-50 border border-assessment-secondary-300 text-assessment-secondary-900 text-sm rounded-lg focus:ring-assessment-accent-500 focus:border-assessment-accent-500 block w-full p-2.5"
                />
                <template v-if="transaction$.total.$error">
                  <span
                    v-for="(error, i) in transaction$.total.$errors"
                    :key="i"
                    class="block text-sm text-assessment-primary-500 font-medium"
                  >
                    {{ error.$message }}
                  </span>
                </template>
              </div>
              <div class="col-span-2">
                <label
                  for="description"
                  class="block mb-2 text-sm font-medium text-assessment-secondary-900"
                  >Product Description</label
                >
                <textarea
                  id="description"
                  rows="4"
                  @input="transaction$.description.$touch()"
                  v-model="transactionForm.description"
                  class="block p-2.5 w-full text-sm text-assessment-secondary-900 bg-assessment-secondary-50 rounded-lg border border-assessment-secondary-300 focus:ring-assessment-accent-500 focus:border-assessment-accent-500"
                  placeholder="Write product description here"
                ></textarea>
              </div>
              <button
                type="submit"
                class="w-full text-white bg-assessment-accent-700 hover:bg-assessment-accent-800 focus:ring-4 focus:outline-none focus:ring-assessment-accent-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center"
              >
                Add Transaction
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
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
import { useCurrency } from '@/extensions/currency.pipe'
import useVuelidate from '@vuelidate/core'
import { rules } from '@/validators/account.validators'
import { transactionRules } from '@/validators/transaction.validator'
import type { Transaction } from '@/models/transaction.model'
import { Modal, type ModalInterface } from 'flowbite'

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
const transactionForm = reactive({
  accountNo: editForm.id,
  description: '',
  orderedDate: new Date(),
  total: 0,
})

let modal: ModalInterface

const transaction$ = useVuelidate(transactionRules, transactionForm)
const v$ = useVuelidate(rules, editForm)

const balance = computed(() => {
  return useCurrency(editForm.balance).formattedPrice.value.toString()
})

onMounted(() => {
  get()
  const $modalEl = document.getElementById('add-transaction')
  if ($modalEl) {
    modal = new Modal($modalEl, {
      closable: true,
      placement: 'center',
      backdrop: 'dynamic',
      backdropClasses: 'bg-gray-900/50 fixed inset-0 z-40',
      onShow: () => {
        $modalEl.classList.add('bg-gray-900/50', 'fixed', 'inset-0', 'z-40')
      },
      onHide: () => {
        $modalEl.classList.remove('bg-gray-900/50', 'fixed', 'inset-0', 'z-40')
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
const onAddTransaction = async () => {
  transaction$.value.$touch()
  if (transaction$.value.$invalid) {
    return
  }
  try {
    transactionForm.accountNo = editForm.id
    const uri = `https://localhost:7297/api/Transactions`
    var response = await axios.post<Response<Transaction>>(uri, transactionForm)
    var result = response.data
    editForm.transactions.push(result.data)
    succeeded.value = false
    message.value = result.message
    modalType.value = 'success'
    modal.hide()
  } catch (e) {
    modalType.value = 'error'
    const $modalEl = document.getElementById('add-transaction')
    if (e instanceof AxiosError && typeof e.response !== 'undefined') {
      var err = e.response.data.errors as string[]
      succeeded.value = false
      messages.value = err.length > 0 ? err : e.response?.data.errors
    } else {
      if ($modalEl) {
        $modalEl.classList.remove('bg-gray-900/50', 'fixed', 'inset-0', 'z-40')
      }
      message.value = 'An unexpected error whil adding a transaction'
    }
  }
}
</script>

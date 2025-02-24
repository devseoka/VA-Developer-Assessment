<template v-if="editForm && editForm.id">
  <div v-if="editForm">
    <Toaster v-if="toast.message" :duration="3000" :message="toast.message" :type="toast.type" />
    <div class="border-y-2 py-4 border-assessment-secondary-300">
      <div class="flex w-full justify-between items-center">
        <TableHeader class="capitalize" :subtitle="'Edit transaction details.'"
          :title="`${editForm.description}'s Transaction`" />
      </div>
      <div class="max-w-2xl px-4 py-8 mx-auto lg:py-16">
        <h2 class="mb-4 text-xl font-bold text-assessment-secondary-900">Update product</h2>
        <form @submit.prevent="onUpdate">
          <AlertError :messages="messages" :succeeded="false" />
          <div class="grid gap-4 mb-4 sm:grid-cols-2 sm:gap-6 sm:mb-5">
            <div class="w-full">
              <label for="dateCaptured" class="block mb-2 text-sm font-medium text-assessment-secondary-900">Date
                Captured</label>
              <input type="datetime-local" id="dateCaptured" aria-label="dateCaptured" v-model="editForm.processedDate"
                class="bg-gray-100 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-assessment-accent-500 focus:border-assessment-accent-500 block w-full p-2.5 cursor-not-allowed"
                disabled readonly>
            </div>
            <div class="w-full">
              <label for="transactionDate"
                class="block mb-2 text-sm font-medium text-assessment-secondary-900">Transaction
                Date</label>
              <input type="datetime-local" id="transactionDate" v-model="editForm.orderedDate"
                @blur="transaction$.orderedDate.$touch()"
                class="bg-assessment-secondary-50 border border-assessment-secondary-300 text-assessment-secondary-900 text-sm rounded-lg focus:ring-assessment-accent-600 focus:border-assessment-accent-600 block w-full p-2.5">
              <template v-if="transaction$.total.$error">
                <span v-for="(error, i) in transaction$.total.$errors" :key="i"
                  class="block text-sm text-assessment-primary-500 font-medium">
                  {{ error.$message }}
                </span>
              </template>
            </div>
            <div class="sm:col-span-2">
              <label for="price" class="block mb-2 text-sm font-medium text-assessment-secondary-900">Transaction
                Amount</label>
              <input type="number" name="price" id="price" v-model="editForm.total" @blur="transaction$.total.$touch()"
                class="bg-assessment-secondary-50 border border-assessment-secondary-300 text-assessment-secondary-900 text-sm rounded-lg focus:ring-assessment-accent-600 focus:border-assessment-accent-600 block w-full p-2.5">
              <template v-if="transaction$.total.$error">
                <span v-for="(error, i) in transaction$.total.$errors" :key="i"
                  class="block text-sm text-assessment-primary-500 font-medium">
                  {{ error.$message }}
                </span>
              </template>
            </div>
            <div class="sm:col-span-2">
              <label for="description"
                class="block mb-2 text-sm font-medium text-assessment-secondary-900">Description</label>
              <textarea id="description" rows="" v-model="editForm.description"
                @blur="transaction$.description.$touch()"
                class="block p-2.5 w-full text-sm text-assessment-secondary-900 bg-assessment-secondary-50 rounded-lg border border-assessment-secondary-300 focus:ring-assessment-accent-500 focus:border-assessment-accent-500">
    {{ editForm.description }}
  </textarea>
              <template v-if="transaction$.total.$error">
                <span v-for="(error, i) in transaction$.total.$errors" :key="i"
                  class="block text-sm text-assessment-primary-500 font-medium">
                  {{ error.$message }}
                </span>
              </template>
            </div>
          </div>
          <div class="flex items-center space-x-4">
            <button type="submit"
              class="text-white w-full bg-assessment-accent-700 hover:bg-assessment-accent-800 focus:ring-4 focus:outline-none focus:ring-assessment-accent-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center">
              Update Transaction
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive, onMounted, nextTick, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import axios, { AxiosError } from 'axios';
import type { Response } from '@/models/response.model';
import Toaster from '@/components/shared/Toaster.vue';
import TableHeader from '@/components/table/TableHeader.vue';
import type { Transaction } from '@/models/transaction.model';
import useVuelidate from '@vuelidate/core';
import { transactionRules } from '@/validators/transaction.validator';
import AlertError from '@/components/shared/AlertError.vue';

const succeeded = ref<boolean>(false)
const messages = ref<string[]>([])

const route = useRoute();
const router = useRouter();
const userId = ref<number>(Number(route.params.id))
const toast = ref<{ message: string, type: string }>({
  message: '',
  type: 'success'
})

const editForm = reactive<Transaction>({
  accountId: 0,
  description: '',
  orderedDate: new Date(),
  total: 0,
  processedDate: new Date(),
  id: 0
})
onMounted(() => {
  get();
});

const get = async () => {
  try {
    const response = await axios.get<Response<Transaction>>(`https://localhost:7297/api/transactions/${userId.value}`);
    const result = response.data;
    Object.assign(editForm, result.data)
    await nextTick();
  } catch (error) {
    router.push('/users');
  }
};

const transaction$ = useVuelidate(transactionRules, editForm)
const onUpdate = async () => {
  try {
    const response = await axios.put<Response<Transaction>>(`https://localhost:7297/api/transactions/`, editForm);
    const result = response.data;
    Object.assign(editForm, result.data)
    toast.value.message = result.message
    toast.value.type = result.succeeded ?
      'success' : 'error'
    succeeded.value = result.succeeded
  }
  catch (e) {
    if (e instanceof AxiosError && typeof e.response !== 'undefined') {
      var err = e.response.data.errors as string[]
      succeeded.value = false
      messages.value = err.length > 0 ? err : e.response?.data.errors
    } else {
      messages.value = ['An unexpected error whil adding a transaction']
    }
  }
}

</script>

<template v-if="editForm && editForm.id">
  <div v-if="editForm">
    <Edit @user-update-event="onUpdated" :user="editForm" />
    <Toaster v-if="toast.message" :duration="3000" :message="toast.message" :type="toast.type" />
    <div class="flex justify-between w-full items-center py-8">
      <div class="flex flex-col">
        <h2 class="text-2xl font-bold text-assessment-secondary-500 capitalize">
          {{ editForm.firstName }} {{ editForm.lastName }}
        </h2>
        <p class="mb-2 text-assessment-primary-500 uppercase">{{ editForm.idNo }}</p>
      </div>
      <button type="button" data-modal-target="edit-user" data-modal-toggle="edit-user"
        class="py-3 px-2 bg-assessment-accent-500 text-center text-assessment-accent-50 font-bold capitalize rounded shadow cursor-pointer focus:ring-4 focus:ring-assessment-accent-300 hover:bg-assessment-accent-700 mx-2 focus:outline-none">Edit
        {{ editForm.lastName }}'s Details</button>
    </div>
    <div class="border-y-2 py-4 border-assessment-secondary-300">
      <div class="flex w-full justify-between items-center">
        <TableHeader class="capitalize" :subtitle="'Manage all transaction related to this users.'"
          :title="`${editForm.lastName}'s Accounts`" />
        <button type="button" data-modal-target="add-account" data-modal-toggle="add-account"
          class="py-3 px-2 bg-assessment-accent-500 rounded-lg text-assessment-accent-50 cursor-pointer hover:bg-assessment-accent-600 focus:ring-4 focus:ring-assessment-accent-300 focus:bg-assessment-accent-700 font-semibold text-sm mx-3 focus:outline-none">
          Add Accounts
        </button>
      </div>
      <div id="accordion-flush" data-accordion="collapse" data-active-classes="bg-white text-assessment-secondary-900"
        data-inactive-classes="text-assessment-secondary-500">
        <h2 id="accordion-flush-heading-1" class="text-2xl font-bold text-assessment-accent-500">
          Accounts
        </h2>
        <h2 v-for="(account, index) in editForm.accounts" :key="account.id" :id="`accordion-flush-heading-${index}`">
          <button type="button"
            class="flex items-center justify-between w-full py-5 font-medium text-left text-assessment-secondary-500 border-b border-assessment-secondary-200 gap-3 px-2 cursor-pointer"
            :data-accordion-target="`#accordion-flush-body-${index}`" aria-expanded="false"
            :aria-controls="`accordion-flush-body-${index}`">
            <span class="font-medium">Account Number: {{ account.accountNo }}</span>
            <svg data-accordion-icon class="w-3 h-3 rotate-180 shrink-0 text-assessment-primary-500" aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 10 6">
              <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="M9 5 5 1 1 5" />
            </svg>
          </button>
        </h2>
        <div v-for="(account, index) in editForm.accounts" :key="account.id" :id="`accordion-flush-body-${index}`"
          class="hidden" :aria-labelledby="`accordion-flush-heading-${index}`">
          <div class="flex flex-col py-5 border-b border-assessment-secondary-200">
            <p class="mb-2 text-assessment-secondary-500 font-semibold">Number of Transactions: {{
              account.transactions?.length || 0 }}</p>
            <p class="mb-2 text-assessment-secondary-500">Balance: {{ useCurrency(account.balance).formattedPrice }}</p>
            <a :href="`/accounts/${account.id}`"
              class="text-sm font-semibold text-assessment-accent-500 hover:text-base hover:text-assessment-accent-700 flex justify-end mx-2">Edit
              Account</a>
          </div>

        </div>
      </div>
    </div>
  </div>
  <Add v-if="editForm.id" :user-id="editForm.id" @account-added-event="onAddedAccount" />
</template>

<script setup lang="ts">
import { reactive, onMounted, nextTick, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import axios from 'axios';
import { initAccordions, Modal, type ModalInterface, type ModalOptions } from 'flowbite';
import type { User } from '@/models/user.model';
import type { Response } from '@/models/response.model';
import { useCurrency } from '@/extensions/currency.pipe';
import Edit from '@/components/modals/users/Edit.vue';
import Toaster from '@/components/shared/Toaster.vue';
import TableHeader from '@/components/table/TableHeader.vue';
import Add from '@/components/modals/accounts/Add.vue';
import type { Account } from '@/models/account.model';

const route = useRoute();
const router = useRouter();
const userId = ref<number>(Number(route.params.id))
const toast = ref<{ message: string, type: string }>({
  message: '',
  type: 'success'
})

const editForm = reactive<User>({
  id: Number(route.params.id),
  firstName: '',
  lastName: '',
  idNo: '',
  accounts: [],
});

onMounted(() => {
  get();
  initModal()
});

const get = async () => {
  try {
    const response = await axios.get<Response<User>>(`https://localhost:7297/api/persons/${userId.value}`);
    const result = response.data;
    const { firstName, lastName, ...rest } = result.data;

    const user = {
      firstName: firstName.toLowerCase(),
      lastName: lastName.toLowerCase(),
      ...rest,
    };

    Object.assign(editForm, { ...editForm, ...user })
    await nextTick();
    initAccordions();
    initModal()
  } catch (error) {
    console.error('Failed to fetch user data:', error);
    router.push('/users');
  }
};
const initModal = () => {
  const $modalEl = document.getElementById('edit-user');
  if ($modalEl) {
    const modalOptions: ModalOptions = {
      closable: true,
      placement: 'center',
      backdrop: 'dynamic',
      backdropClasses:
        'fixed inset-0 z-40',
      onShow: () => {
        $modalEl.classList.add('bg-gray-900/50', 'fixed', 'inset-0', 'z-40');
      },
      onHide: () => {
        get()
      }
    }
  }
}
const onUpdated = (response: Response<User>) => {
  Object.assign(editForm, response.data)
  toast.value.message = response.message
  toast.value.type = response.succeeded ?
    'success' : 'error'
  initModal()
}
const onAddedAccount = (response: Response<Account>) => {
  const $accountEl = document.getElementById('add-account')
  if ($accountEl) {
    editForm.accounts.push(response.data)
    toast.value.message = response.message
    toast.value.type = response.succeeded ?
      'success' : 'error'
    $accountEl.click()
  }
}
</script>

<template>
  <div v-if="editForm">
    <Edit @user-update-event="onUpdated" :user="editForm" :modal="modal" />
    <Toaster v-if="toast.message" :duration="3000" :message="toast.message" :type="toast.type" />
    <div class="flex justify-between w-full items-center">
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
          <a :href="`/accounts/${account.id}`" class="text-sm font-semibold text-assessment-accent-500 hover:text-base hover:text-assessment-accent-700 flex justify-end mx-2">Edit Account</a>
        </div>

      </div>
    </div>
  </div>
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

const route = useRoute();
const router = useRouter();
const userId = route.params.id;
let modal: ModalInterface;
const toast = ref<{ message: string, type: string }>({
  message: '',
  type: 'success'
})

const editForm = reactive<User>({
  id: 0,
  firstName: '',
  lastName: '',
  idNo: '',
  accounts: [],
});

onMounted(() => {
  get();
});

const get = async () => {
  try {
    const response = await axios.get<Response<User>>(`https://localhost:7297/api/persons/${userId}`);
    const result = response.data;
    const { firstName, lastName, ...rest } = result.data;

    const user = {
      firstName: firstName.toLowerCase(),
      lastName: lastName.toLowerCase(),
      ...rest,
    };

    Object.assign(editForm, user);
    await nextTick();
    initAccordions();
    initModal()
  } catch (error) {
    console.error('Failed to fetch user data:', error);
    router.push('/users');
  }
};
const onShow = () => {
  if (modal) {
    modal.show()
  }
}
const initModal = () => {
  const $modalEl = document.getElementById('edit-user');
  if ($modalEl) {
    const modalOptions: ModalOptions = {
      closable: true,
      placement: 'center',
      backdrop: 'dynamic',
      backdropClasses:
        'bg-gray-900/50 dark:bg-gray-900/80 fixed inset-0 z-40',
      onShow: () => {
        $modalEl.classList.add('bg-gray-900/50', 'fixed', 'inset-0', 'z-40');
      },
      onHide: () => {
        get()
        $modalEl.classList.remove('bg-gray-900/50', 'fixed', 'inset-0', 'z-40');
      }
    }
    modal = new Modal($modalEl, modalOptions);
  }
}
const onUpdated = (response: Response<User>) => {
  Object.assign(editForm, response.data)
  toast.value.message = response.message
  toast.value.type = response.succeeded ?
    'success' : 'error'
  initModal()
}
</script>

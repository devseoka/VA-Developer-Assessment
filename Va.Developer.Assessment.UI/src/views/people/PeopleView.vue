<template>
  <div class=""></div>
  <Add @user-added-event="add" :modal="modal" />
  <TableHeader :title="'Assessment Users'" :subtitle="'Manage or add new users to the assessement application'" />
  <div class="flex flex-col md:flex-row items-center justify-between space-y-3 md:space-y-0 md:space-x-4 p-4">
    <div class="w-full md:w-1/2">
      <form class="flex items-center">
        <label for="simple-search" class="sr-only">Search</label>
        <div class="relative w-full">
          <div class="absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
            <svg aria-hidden="true" class="w-5 h-5 text-gray-500" fill="currentColor" viewbox="0 0 20 20"
              xmlns="http://www.w3.org/2000/svg">
              <path fill-rule="evenodd"
                d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z"
                clip-rule="evenodd" />
            </svg>
          </div>
          <input type="text" id="search" v-model="query" @input="onSearch(query)"
            class="bg-gray-50 border border-gray-300 text-assessment-secondary-700 text-sm rounded-lg focus:ring-assessment-accent-500 focus:border-assessment-accent-500 block w-full pl-10 p-2 "
            placeholder="Search" required>
        </div>
      </form>
    </div>
    <div
      class="w-full md:w-auto flex flex-col md:flex-row space-y-2 md:space-y-0 items-stretch md:items-center justify-end md:space-x-3 flex-shrink-0">
      <button type="button" data-modal-target="add-user" data-modal-toggle="add-user" @click="onShow"
        class="flex items-center justify-center text-white bg-assessment-secondary-500 hover:bg-assessment-secondary-800 focus:ring-4 focus:ring-primary-300 font-medium rounded-lg text-sm px-4 py-2 focus:outline-none">
        Add new user
      </button>
    </div>
  </div>
  <div class="overflow-x-auto">
    <table class="w-full text-sm text-left text-gray-500">
      <thead class="text-xs text-assessment-secondary-700 uppercase bg-gray-100">
        <tr>
          <th scope="col" class="px-4 py-3">Id Number</th>
          <th scope="col" class="px-4 py-3">Name</th>
          <th scope="col" class="px-4 py-3">Accounts</th>
          <th scope="col" class="px-4 py-3">
            <span class="sr-only">Actions</span>
          </th>
        </tr>
      </thead>
      <tbody class="cursor-pointer">
        <tr class="border-b" v-for="user in filteredUsers" :key="user.id">
          <th scope="row" class="px-4 py-3 font-medium text-gray-900 whitespace-nowrap">{{ user.idNo }}</th>
          <td class="px-4 py-3 capitalize">{{ user.firstName }} {{ user.lastName }}</td>
          <td class="px-4 py-3" v-for="account in user.accounts">
            <span class="flex flex-col">{{ account.accountNo }}</span>
          </td>
          <td class="px-4 py-3 flex items-center justify-end">
            <button :id="'dropdown-button-' + user.id" :data-dropdown-toggle="'dropdown-' + user.id"
              class="inline-flex items-center p-0.5 text-sm font-medium text-center text-assessment-secondary-500 hover:text-assessment-accent-800 rounded-lg focus:outline-none cursor-pointer"
              type="button">
              <svg class="w-5 h-5" aria-hidden="true" fill="currentColor" viewbox="0 0 20 20"
                xmlns="http://www.w3.org/2000/svg">
                <path
                  d="M6 10a2 2 0 11-4 0 2 2 0 014 0zM12 10a2 2 0 11-4 0 2 2 0 014 0zM16 12a2 2 0 100-4 2 2 0 000 4z" />
              </svg>
            </button>
            <div :id="'dropdown-' + user.id"
              class="hidden z-10 w-44 bg-white rounded divide-y divide-assessment-accent-500 shadow">
              <ul class="py-1 text-sm text-gray-700" :aria-labelledby="'dropdown-button-' + user.id">
                <li>
                  <a href="#" class="block py-2 px-4 hover:bg-assessment-accent-100">Edit</a>
                </li>
              </ul>
              <div class="py-1">
                <a @click.prevent="onDelete(user)"
                  class="block py-2 px-4 text-sm text-assessment-primary-500 hover:bg-assessment-primary-100">Delete</a>
              </div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  <TableFooter :total="total" :itemsPerPage="itemsPerPage" v-model:currentPage="currentPage" />
  <Toaster v-if="toastType" :type="toastType" :message="message" />
</template>
<script setup lang="ts">
import { computed, nextTick, onMounted, ref, watch } from 'vue';
import Fuse from "fuse.js";
import { Dropdown, Modal, type ModalInterface, type ModalOptions } from 'flowbite';
import Add from '@/components/modals/User/Add.vue';
import axios, { AxiosError } from 'axios';
import TableFooter from '@/components/table/TableFooter.vue';
import TableHeader from '@/components/table/TableHeader.vue';
import Toaster from '@/components/shared/Toaster.vue';
import type { User } from '@/models/user.model';
import type { Response } from '@/models/response.model';


const query = ref<string>('');
const message = ref<string>('');
const succeeded = ref<boolean>(false);
const toastType = ref<string>('')
const itemsPerPage = ref<number>(10)
const currentPage = ref<number>(1)

const searchResults = ref<User[]>([])
const users = ref<User[]>([])
const fuse = ref<Fuse<User> | null>(null);

const $modalEl = document.querySelector('#add-user') as HTMLElement;
const modalOptions: ModalOptions = {
  placement: 'center-right',
  backdrop: 'dynamic',
  closable: false,
  backdropClasses: 'bg-gray-900/50 dark:bg-gray-900/80 fixed inset-0 z-40'
}
let modal: ModalInterface

const initFuse = () => {
  if (users.value && users.value.length > 0) {
    fuse.value = new Fuse(users.value, {
      keys: [
        'lastName',
        'firstName',
        'accounts.accountNo'
      ],
      includeScore: true,
      threshold: 0.3
    });
  }

  nextTick(() => {
    users.value.forEach(user => {
      const $targetEl = document.getElementById('dropdown-' + user.id)
      const $triggerEl = document.getElementById('dropdown-button-' + user.id)

      if ($targetEl && $triggerEl) {
        new Dropdown($targetEl, $triggerEl, {
          placement: 'bottom',
          triggerType: 'click'
        })
      }
    })
  })
};
const getUsers = async () => {
  try {
    var response = (await axios.get<Response<User[]>>('http://localhost:5209/api/persons'))
    users.value = response.data.data.map((user) => {
      return {
        ...user,
        firstName: user.firstName.toLowerCase(),
        lastName: user.lastName.toLowerCase(),
      };
    });
  }
  catch (e) {
  }
};
const total = computed(() => users.value.length);
onMounted(() => {
  getUsers();
  if ($modalEl) {
    modal = new Modal($modalEl, modalOptions);
  }
});
const onShow = () => {
  if ($modalEl) {
    modal.show()
  }
}

const onSearch = (search: string) => {
  if (search.trim() !== '' && users.value.length > 0 && fuse.value) {
    const results = fuse.value.search(search);
    searchResults.value = results.map((result) => result.item);
    currentPage.value = 1;
  }
}
const filteredUsers = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage.value;
  const end = start + itemsPerPage.value;
  const source = query.value.trim() !== '' ? searchResults.value : users.value;
  return source.slice(start, end);
});
const add = (user: User) => {
  users.value.push(user);
  modal.hide();
}
const onDelete = async (user:  User) => {
  try
  {
    const response = await axios.delete(`http://localhost:5209/api/persons/${user.id}`);
    if(response.status === 204)
    {
      const index = users.value.findIndex((u) => u.id === user.id);
      users.value.splice(index, 1);
      message.value = `${user.firstName} ${user.lastName} record was deleted successfully`;
      succeeded.value = true;
      toastType.value = 'success';
    }
  }
  catch(e){
    message.value = `An unexpected error occured while deleting ${user.firstName} ${user.lastName} record.`;
    succeeded.value = false;
    toastType.value = 'error';
  }

}

watch(users, initFuse, { immediate: true });

watch(filteredUsers, () => {
  nextTick(() => {
    filteredUsers.value.forEach(user => {
      const $targetEl = document.getElementById('dropdown-' + user.id);
      const $triggerEl = document.getElementById('dropdown-button-' + user.id);

      if ($targetEl && $triggerEl) {
        new Dropdown($targetEl, $triggerEl, {
          placement: 'bottom',
          triggerType: 'click'
        });
      }
    });
  });
});
</script>

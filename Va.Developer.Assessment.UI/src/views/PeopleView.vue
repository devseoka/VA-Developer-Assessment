<template>
  <div class=""></div>
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
            <button id="apple-imac-27-dropdown-button" data-dropdown-toggle="apple-imac-27-dropdown"
              class="inline-flex items-center p-0.5 text-sm font-medium text-center text-gray-500 hover:text-gray-800 rounded-lg focus:outline-none dark:text-gray-400 dark:hover:text-gray-100"
              type="button">
              <svg class="w-5 h-5" aria-hidden="true" fill="currentColor" viewbox="0 0 20 20"
                xmlns="http://www.w3.org/2000/svg">
                <path
                  d="M6 10a2 2 0 11-4 0 2 2 0 014 0zM12 10a2 2 0 11-4 0 2 2 0 014 0zM16 12a2 2 0 100-4 2 2 0 000 4z" />
              </svg>
            </button>
            <div id="apple-imac-27-dropdown"
              class="hidden z-10 w-44 bg-white rounded divide-y divide-gray-100 shadow dark:bg-gray-700 dark:divide-gray-600">
              <ul class="py-1 text-sm text-gray-700 dark:text-gray-200" aria-labelledby="apple-imac-27-dropdown-button">
                <li>
                  <a href="#"
                    class="block py-2 px-4 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">Edit</a>
                </li>
              </ul>
              <div class="py-1">
                <a href="#"
                  class="block py-2 px-4 text-sm text-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-200 dark:hover:text-white">Delete</a>
              </div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  <TableFooter :current-page="1" :total="20" :items-per-page="10" />
  <Add @user-added-event="add" :modal="modal" />
</template>
<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import Fuse from "fuse.js";
import { Modal, type ModalInterface, type ModalOptions } from 'flowbite';
import Add from '@/components/modals/Add.vue';
import axios from 'axios';
import TableFooter from '@/components/table/TableFooter.vue';
import TableHeader from '@/components/table/TableHeader.vue';
import type { User } from '@/models/user.model';
import type { Response } from '@/models/response.model';


const query = ref<string>('');

const itemsPerPage = ref<number>(10)
const currentPage = ref<number>(1)
const total = ref<number>(0)

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
};

watch(users, initFuse, { immediate: true });

const getUsers = () => {
  axios.get<Response<User[]>>('http://localhost:5209/api/persons').then((response) => {
    users.value = response.data.data.map((user) => {
      return {
        ...user,
        firstName: user.firstName.toLowerCase(),
        lastName: user.lastName.toLowerCase(),
      };
    });
    total.value = users.value.length
  }).catch((err) => {
    console.error('An unexpected error occurred while retrieving users:', JSON.stringify(err));
  })
};
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
  total.value = users.value.length;
  modal.hide();
}
</script>

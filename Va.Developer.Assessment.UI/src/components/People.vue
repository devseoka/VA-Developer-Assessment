<template>
  <section class="flex items-center">
    <div class="w-full max-w-screen-xl px-4 mx-auto lg:px-12">
      <div class="relative overflow-hidden bg-white shadow-md sm:rounded-lg">
        <div class="flex-row items-center justify-between p-4 space-y-3 sm:flex sm:space-y-0 sm:space-x-4">
          <div>
            <h4 class="mr-3 font-semibold text-assessment-secondary-700">Assessment Users</h4>
            <p class="text-assessment-secondary-500 text-xs">Manage all existing assessment users or add new user</p>
          </div>

        </div>
        <div class="flex flex-col items-center justify-between p-4 space-y-3 md:flex-row md:space-y-0 md:space-x-4">
          <div class="w-full md:w-1/2">
            <form @submit.prevent="onSearch" class="flex items-center">
              <label for="search" class="sr-only">Search</label>
              <div class="relative w-full">
                <div class="absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
                  <svg aria-hidden="true" class="w-5 h-5 text-gray-500" fill="currentColor" viewbox="0 0 20 20"
                    xmlns="http://www.w3.org/2000/svg">
                    <path fill-rule="evenodd"
                      d="M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z"
                      clip-rule="evenodd" />
                  </svg>
                </div>
                <input type="search" id="search"
                  class="block w-full p-2 pl-10 text-sm text-assessment-secondary-900 border rounded-lg border-assessment-secondary-300 bg-gray-50 focus:ring-assessment-accent-500 focus:border-assessment-accent-500"
                  placeholder="Search" required>
              </div>
            </form>
          </div>
          <div class="flex flex-col items-stretch justify-end shrink-0 w-full">
            <button
              class="items-center justify-center bg-assessment-primary-500 text-assessment-primary-50 px-4 py-2 rounded-lg font-medium hover:bg-assessment-primary-700 flex capitalize cursor-pointer focus:ring-4 focus:ring-assessment-primary-300 focus:outline-none w-full">
              <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 mr-2 -ml-1" viewBox="0 0 20 20" fill="currentColor"
                aria-hidden="true">
                <path
                  d="M8 9a3 3 0 100-6 3 3 0 000 6zM8 11a6 6 0 016 6H2a6 6 0 016-6zM16 7a1 1 0 10-2 0v1h-1a1 1 0 100 2h1v1a1 1 0 102 0v-1h1a1 1 0 100-2h-1V7z" />
              </svg>add assessment user</button>
          </div>
        </div>
        <nav>
          <span>Showing <span>1 - {{ totalPages }}</span> of
            <span>{{ total }}</span>
            <ul>
              <li>
                <a @click.prevent="onPrevious" v-if="currentPage > 1">
                  <span>Previous</span>
                  <svg class="w-5 h-5" aria-hidden="true" fill="currentColor" viewBox="0 0 20 20"
                    xmlns="http://www.w3.org/2000/svg">
                    <path fill-rule="evenodd"
                      d="M12.707 5.293a1 1 0 010 1.414L9.414 10l3.293 3.293a1 1 0 01-1.414 1.414l-4-4a1 1 0 010-1.414l4-4a1 1 0 011.414 0z"
                      clip-rule="evenodd"></path>
                  </svg>
              <li>
                <a @click.prevent="onNext" v-if="currentPage === totalPages"
                  class="flex items-center justify-center px-3 py-2 text-sm leading-tight text-gray-500 bg-white border border-gray-300 hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white">1</a>
              </li>
              </a>
              </li>
            </ul>
          </span>
        </nav>
      </div>
    </div>
  </section>
</template>
<script setup lang="ts">
import type { User } from '@/models/user.model';
import type { Response } from '@/models/response.model';
import { computed, onMounted, ref } from 'vue';

const users = ref<User[]>([])
const total = ref<number>(0)
const itemsPerPage = ref<number>(10)
const currentPage = ref(1)

const getUsers = async () => {
  try {
    const response = await fetch(`http://localhost:5209/api/persons`);
    const result: Response<User[]> = await response.json();
    users.value = result.data
    users.value = users.value.map((user) => {
      return {
        ...user,
        firstName: user.firstName.toLowerCase(),
        lastName: user.lastName.toLowerCase(),
      };
    });
    total.value = users.value.length
  } catch (error) {
    console.error('An unexpected error occurred while retrieving users:', JSON.stringify(error));
  }
};
onMounted(() => {
  getUsers();
});

const onSearch = () => {

}
const onPrevious = () => {
  if (currentPage.value > 1) currentPage.value--;
}
const onNext = () => {
  if (currentPage.value < totalPages.value) currentPage.value++;
}
const totalPages = computed(() => {
  return Math.ceil(total.value / itemsPerPage.value);
});
const filteredUsers = computed(() => {
  const start = (currentPage.value - 1) * itemsPerPage.value;
  const end = start + itemsPerPage.value;
  return users.value.slice(start, end);
});

</script>

<template>
  <nav class="flex flex-col md:flex-row justify-between items-start md:items-center space-y-3 md:space-y-0 p-4"
    aria-label="Table navigation">
    <span class="text-sm font-normal text-gray-500 dark:text-gray-400">
      Showing
      <span class="font-semibold text-assessment-secondary-900">
        {{ currentPage }} &dash; {{ totalPages }}
      </span>
      of
      <span class="font-semibold text-assessment-primary-500">{{ total }}</span>
    </span>
    <ul class="inline-flex items-stretch -space-x-px cursor-pointer">
      <li @click.prevent="onPrevious" :class="{ 'opacity-50 cursor-not-allowed': currentPage === 1 }">
        <a
          class="flex items-center justify-center h-full py-1.5 px-3 ml-0 text-assessment-secondary-500 bg-white rounded-l-lg border border-gray-300 hover:bg-assessment-secondary-100 hover:text-assessment-secondary-700">
          <span class="sr-only">Previous</span>
          <svg class="w-5 h-5" aria-hidden="true" fill="currentColor" viewbox="0 0 20 20"
            xmlns="http://www.w3.org/2000/svg">
            <path fill-rule="evenodd"
              d="M12.707 5.293a1 1 0 010 1.414L9.414 10l3.293 3.293a1 1 0 01-1.414 1.414l-4-4a1 1 0 010-1.414l4-4a1 1 0 011.414 0z"
              clip-rule="evenodd" />
          </svg>
        </a>
      </li>
      <li v-for="page in totalPages" :key="page">
        <a href="#" :class="{
          'flex items-center justify-center text-sm py-2 px-3 leading-tight': true,
          'text-gray-500 bg-white border border-assessment-secondary-300 hover:bg-assessment-secondary-100 hover:text-assessment-secondary-700':
            currentPage !== page,
          'text-assessment-secondary-500 bg-assessment-accent-50 border border-assessment-accent-300 hover:bg-assessment-accent-500 hover:text-assessment-secondary-50':
            currentPage === page,
        }" @click.prevent="redirectPage(page)">
          {{ page }}
        </a>
      </li>
      <li @click.prevent="onNext" :class="{ 'opacity-50 cursor-not-allowed': currentPage === totalPages }">
        <a
          class="flex items-center justify-center h-full py-1.5 px-3 leading-tight text-gray-500 bg-white rounded-r-lg border border-gray-300 hover:bg-gray-100 hover:text-gray-700 dark:bg-gray-800 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white">
          <span class="sr-only">Next</span>
          <svg class="w-5 h-5" aria-hidden="true" fill="currentColor" viewbox="0 0 20 20"
            xmlns="http://www.w3.org/2000/svg">
            <path fill-rule="evenodd"
              d="M7.293 14.707a1 1 0 010-1.414L10.586 10 7.293 6.707a1 1 0 011.414-1.414l4 4a1 1 0 010 1.414l-4 4a1 1 0 01-1.414 0z"
              clip-rule="evenodd" />
          </svg>
        </a>
      </li>
    </ul>
  </nav>
</template>

<script setup lang="ts">
import { computed, defineProps, defineEmits } from "vue";

const props = defineProps<{
  total: number;
  itemsPerPage: number;
  currentPage: number;
}>();

const emit = defineEmits(["update:currentPage"]);

const totalPages = computed(() => Math.ceil(props.total / props.itemsPerPage));

const redirectPage = (page: number) => {
  emit("update:currentPage", page);
};

const onPrevious = () => {
  if (props.currentPage > 1) emit("update:currentPage", props.currentPage - 1);
};

const onNext = () => {
  if (props.currentPage < totalPages.value) emit("update:currentPage", props.currentPage + 1);
};
</script>

<template>
  <div :id="`${type}-toaster`" v-if="visible" :class="[
    ' fixed top-5 left-5 divide-x rtl:divide-x-reverse divide-gray-200 rounded-lg shadow-sm flex items-center w-full max-w-xs p-4 mb-4 text-gray-500 bg-white',
    toastTypeClass
  ]" role="alert">
    <div :class="iconContainerClass">
      <svg v-if="type === 'success'" class="w-5 h-5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg"
        fill="currentColor" viewBox="0 0 20 20">
        <path
          d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5Zm3.707 8.207-4 4a1 1 0 0 1-1.414 0l-2-2a1 1 0 0 1 1.414-1.414L9 10.586l3.293-3.293a1 1 0 0 1 1.414 1.414Z" />
      </svg>
      <svg v-if="type === 'error'" class="w-5 h-5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg"
        fill="currentColor" viewBox="0 0 20 20">
        <path
          d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5Zm3.707 11.793a1 1 0 1 1-1.414 1.414L10 11.414l-2.293 2.293a1 1 0 0 1-1.414-1.414L8.586 10 6.293 7.707a1 1 0 0 1 1.414-1.414L10 8.586l2.293-2.293a1 1 0 0 1 1.414 1.414L11.414 10l2.293 2.293Z" />
      </svg>
      <svg v-if="type === 'warning'" class="w-5 h-5" aria-hidden="true" xmlns="http://www.w3.org/2000/svg"
        fill="currentColor" viewBox="0 0 20 20">
        <path
          d="M10 .5a9.5 9.5 0 1 0 9.5 9.5A9.51 9.51 0 0 0 10 .5ZM10 15a1 1 0 1 1 0-2 1 1 0 0 1 0 2Zm1-4a1 1 0 0 1-2 0V6a1 1 0 0 1 2 0v5Z" />
      </svg>
    </div>
    <div class="ms-3 text-sm font-normal capitalize">{{ message }}</div>
    <button type="button"
      class="ms-auto -mx-1.5 -my-1.5 bg-white text-gray-400 hover:text-gray-900 rounded-lg focus:ring-2 focus:ring-gray-300 p-1.5 hover:bg-gray-100 inline-flex items-center justify-center h-8 w-8"
      @click="closeToast" aria-label="Close">
      <svg class="w-3 h-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
        <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
          d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6" />
      </svg>
    </button>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, defineProps, watch, onMounted } from 'vue';
import { Dismiss } from 'flowbite';
import type { DismissOptions } from 'flowbite';

const props = defineProps({
  type: { type: String, required: true },
  message: { type: String, required: true },
  duration: { type: Number, default: 3000 }
});

const visible = ref(true);

const toastTypeClass = computed(() => {
  return {
    success: 'border-l-4 border-assessment-accent-500',
    error: 'border-l-4 border-assessment-primary-500',
  }[props.type] || '';
});

const iconContainerClass = computed(() => {
  return {
    success: 'text-assessment-accent-500 bg-assessment-accent-100',
    error: 'text-assessment-primary-500 bg-assessment-primary-100',
    warning: 'text-orange-500 bg-orange-100'
  }[props.type] || '';
});

const closeToast = () => {
  visible.value = false;
};

// Programmatically dismiss toast
onMounted(() => {
  const toastEl = document.getElementById(`${props.type}-toaster`);
  if (toastEl) {
    const dismissOptions: DismissOptions = {
      transition: 'transition-opacity',
      duration: 1000,
      timing: 'ease-out',
      onHide: () => {
        console.log('Toast has been dismissed.');
      }
    };

    const dismissInstance = new Dismiss(toastEl, undefined, dismissOptions);
    setTimeout(() => {
      dismissInstance.hide();
    }, props.duration);  // Dismiss after duration
  }
});
</script>

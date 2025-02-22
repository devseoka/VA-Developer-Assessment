<template>
  <div class="relative text-assessment-secondary-800 overflow-hidden">
    <div class="flex-row items-center justify-between p-4 space-y-3 sm:flex sm:space-y-0 sm:space-x-4">
      <div>
        <h5 class="mr-3 font-semibold">{{ props.title }}</h5>
        <p class="text-assessment-secondary-500">{{ props.subtitle }}</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
const props = defineProps<{
  subtitle: string;
  title: string;
}>();
</script>

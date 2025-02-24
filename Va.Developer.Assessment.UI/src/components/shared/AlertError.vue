<template>
  <div v-if="messages.length > 0" class="col-span-2 text-center">
    <p v-for="(message, index) in messages" :key="index" :class="succeeded ?
      'mt-2 text-sm text-assessment-accent-600 capitalize'
      : 'mt-2 text-sm text-assessment-primary-600 text-left'">
      {{ message }}
    </p>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';

const props = defineProps({
  messages: {
    type: Array as () => string[],
    required: true,
  },
  succeeded: {
    type: Boolean,
    required: true
  }
});

const messages = computed(() => props.messages ?? []);

</script>

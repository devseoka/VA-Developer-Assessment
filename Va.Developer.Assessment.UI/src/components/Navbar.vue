<template>
  <nav class="flex justify-between items-center w-full bg-slate-50  p-4 shadow-lg">
    <a :href="'/'" class="flex items-center text-assessment-primary-500">
      <img :src="logo" alt="Moshele Seoka's Logo" class="h-10 w-10 mr-2 fill-assessment-primary-500">
      <span class="font-semibold text-xl tracking-tight">VA Assessment</span>
    </a>
    <div class="hidden md:flex space-x-4">
      <a class="block hover:text-assessment-accent-500 hover:font-semibold transition-colors duration-300 ease-in-out" :href="'/about'">About</a>
      <a class="block hover:text-assessment-accent-500 hover:font-semibold transition-colors duration-300 ease-in-out" :href="'/contact'">Contact</a>
      <a class="block hover:text-assessment-accent-500 hover:font-semibold transition-colors duration-300 ease-in-out" :href="'/users'">People</a>

    </div>
  </nav>
</template>
<script setup lang="ts">
import logo from '@/assets/official-logo.svg';
</script>

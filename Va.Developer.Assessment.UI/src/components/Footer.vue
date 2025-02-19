<template>
 <footer class="p-7 bg-slate-50 text-assessment-secondary-500 text-center">
  <span>&copy; {{ year }} <a class="font-bold hover:text-assessment-accent-500 transition-colors duration-300 ease-in-out" :href="link">Seoka Moshele Kabelo</a>. All rights reserved.</span>
 </footer>
</template>
<script setup lang="ts">
import { ref } from 'vue';

 const year = ref(new Date().getFullYear())
 const link =  ref('https://seokamoshele.digital')
</script>

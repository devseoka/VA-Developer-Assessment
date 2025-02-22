<template>
  <Navbar />
  <main class="flex-grow p-6">
    <People />
  </main>
  <Footer />
</template>
<script setup lang="ts">
import  Navbar  from '@/components/Navbar.vue'
import  Footer  from '@/components/Footer.vue'
import  People from '@/components/People.vue';
import { onMounted } from 'vue';
import { initFlowbite } from 'flowbite';


onMounted(() => {
  initFlowbite()
})
</script>


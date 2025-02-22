<template>
  <Navbar />
  <main class="flex-grow p-6">
    <section class="bg-gray-50 p-3 sm:p-5 shadow-md sm:rounded-lg">
      <div class="mx-auto max-w-screen-xl px-4 lg:px-12">
        <div class="relative  overflow-hidden">
          <RouterView />
        </div>
      </div>
    </section>
  </main>
  <Footer />
</template>
<script setup lang="ts">
import Navbar from '@/components/Navbar.vue'
import Footer from '@/components/Footer.vue'
import { onMounted } from 'vue';
import { initFlowbite } from 'flowbite';
import { RouterView } from 'vue-router';

onMounted(() => {
  initFlowbite()
})
</script>

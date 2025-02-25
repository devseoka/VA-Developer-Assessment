<template>
  <section class="py-12">
    <div class="max-w-3xl mx-auto text-center">
      <h2 class="text-3xl font-bold text-assessment-primary-500">About Us</h2>
      <p class="mt-2 text-assessment-secondary-600">We built this application using a modern and powerful tech stack to ensure
        efficiency, scalability, and a seamless user experience.</p>

      <div class="grid grid-cols-1  md:grid-cols-2 gap-6 mt-6 place-items-center">
        <img :src="NetImg" alt="Vue" class="w-32 h-32">
        <img :src="TypescriptImg" alt="TypeScript" class="w-32 h-32">
        <img :src="TailwindImg" alt="Tailwind" class="w-32 h-32">
        <img :src="flowbiteImg" alt="Flowbite" class="w-32 h-32">
      </div>
    </div>
  </section>

</template>
<script setup lang="ts">
 import flowbiteImg from '@/assets/flowbite-logo.svg'
 import TailwindImg from '@/assets/tailwindcss-mark.d52e9897.svg'
 import TypescriptImg from '@/assets/ts-logo-256.svg'
  import NetImg from '@/assets/dotnet-official.svg'
</script>

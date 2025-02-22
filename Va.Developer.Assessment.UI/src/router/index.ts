import { createRouter, createWebHistory } from 'vue-router';
import PeopleView from '@/views/PeopleView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/users', name: 'users', component: PeopleView },
  ],
});

export default router;

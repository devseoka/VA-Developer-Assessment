import { createRouter, createWebHistory } from 'vue-router';
import PeopleView from '@/views/people/PeopleView.vue';
import EditView from '@/views/people/EditView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/users', name: 'users', component: PeopleView },
    { path: '/users/:id', name: 'user-detail', component: EditView },
  ],
});

export default router;

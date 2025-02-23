import { createRouter, createWebHistory } from 'vue-router';
import PeopleView from '@/views/people/PeopleView.vue';
import EditAccount from '@/views/account/EditAccountView.vue'
import EditPersonView from '@/views/people/EditPersonView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/users', name: 'users', component: PeopleView },
    { path: '/users/:id', name: 'user-detail', component: EditPersonView },
    { path: '/accounts/:id', name: 'account-detail', component: EditAccount },
  ],
});

export default router;

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import type { task } from './models/task.model'
const name = ref('Virtual Agent Developer Assessment | Seoka Moshele')
const status = ref<string>('pending')
const tasks = ref<string[]>([])
const year = ref(new Date().getFullYear())
const link = 'https://seokamoshele.digital'

const task = ref('')
const onClick = () => {
  if (status.value === 'active') {
    status.value = 'pending';
  } else if (status.value === 'pending') {
    status.value = 'disabled';
  } else {
    status.value = 'active';
  }
};
const onSubmit = () => {
  if (task.value.trim() !== '') {
    tasks.value.push(task.value)
    task.value = ''
  }
}
const onDelete = (task: string) => {
  tasks.value.filter((t) => t === task)
  tasks.value = tasks.value.filter((t) => t !== task);
}
onMounted(async () => {
    try
    {
      const response = await fetch('https://jsonplaceholder.typicode.com/todos')
      const data: task[] = await response.json()
      tasks.value.push(...data.map((t) => t.title))
    }
    catch(e){
      console.log(`An unexpected error occured`, e)
    }
})

</script>
<template>
  <h1>{{ name }}</h1>
  <form @submit.prevent="onSubmit">
    <div class="form-group">
      <label for="task">Task</label>
      <input type="text" name="task" id="task" v-model="task">
    </div>
    <button type="submit">Add New Task</button>
  </form>
  <div v-if="tasks.length > 0">
    <h3>Tasks</h3>
    <ul>
      <li v-for="task in tasks" :key="task" class="task-item">
        <span>{{ task }}</span>
        <button class="delete-button" @click="onDelete(task)">X</button>
      </li>

      <!-- <a v-bind:href="link">Seoka Moshele</a> -->
      <button style="display: block;margin: 24px 0;" @click="onClick">Change Status</button>
    </ul>
  </div>
  <p v-else>You are yet to add a task</p>
  <footer class="footer">
    <p>&copy; {{ year }}. <a :href="link" target="_blank" rel="noopener noreferrer">Seoka Moshele's Portfolio</a></p>
  </footer>


</template>

<style scoped>
.task-item {
  display: flex;
  /* Use flexbox to arrange the span and button horizontally */
  align-items: center;
  /* Vertically center the items */
  justify-content: space-between;
  /* Space out the task and delete button */
  padding: 10px 15px;
  margin: 5px 0;
  background-color: #f9f9f9;
  /* Light background */
  border-radius: 5px;
  /* Rounded corners */
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  /* Subtle shadow for depth */
}

.task-item span {
  flex-grow: 1;
  /* This ensures the task takes up the remaining space */
  font-size: 1rem;
  color: #333;
  /* Dark text color */
  font-weight: 500;
}

.delete-button {
  background-color: #e74c3c;
  /* Red background */
  color: white;
  /* White text */
  border: none;
  border-radius: 50%;
  width: 30px;
  height: 30px;
  font-size: 1.2rem;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background-color 0.3s, transform 0.2s;
}

/* Hover and focus effects for the delete button */
.delete-button:hover,
.delete-button:focus {
  background-color: #c0392b;
  /* Darker red on hover/focus */
  transform: scale(1.1);
  /* Slightly enlarge the button on hover */
}

.delete-button:active {
  transform: scale(0.9);
  /* Shrink on click */
}

.delete-button:focus {
  outline: none;
  /* Remove the default focus outline */
  box-shadow: 0 0 3px 2px rgba(231, 76, 60, 0.5);
  /* Optional: Add a glow effect when focused */
}

/* Footer styling */
.footer {
  color: #2c3e50;
  /* Dark background */
  text-align: center;
  padding: 20px;
  font-size: 0.9rem;
  /* Slightly smaller font size */
  position: relative;
  bottom: 0;
  width: 100%;
}

.footer p {
  margin: 0;
  line-height: 1.5;
}

.footer a {
  color: #1abc9c;
  /* Greenish link color */
  text-decoration: none;
  font-weight: bold;
  transition: color 0.3s ease;
}

.footer a:hover {
  color: #16a085;
  /* Darker green on hover */
}

/* Optional: Add spacing for footer if it's at the bottom of the page */
body {
  margin-bottom: 50px;
  /* Adjust this as needed */
}

.form-group label {
  font-size: 1rem;
  font-weight: bold;
  color: #333;
  margin-bottom: 0.5rem;
}

/* Styling for the input inside form-group */
.form-group input,
.form-group select,
.form-group textarea {
  padding: 0.8rem;
  border: 1px solid #ccc;
  border-radius: 5px;
  font-size: 1rem;
  width: 100%;
  box-sizing: border-box;
  /* Ensures padding does not affect width */
  transition: border-color 0.3s ease;
}

/* Focus state for inputs inside form-group */
.form-group input:focus,
.form-group select:focus,
.form-group textarea:focus {
  border-color: #4CAF50;
  /* Green border on focus */
  outline: none;
}

/* Add a subtle hover effect */
.form-group input:hover,
.form-group select:hover,
.form-group textarea:hover {
  border-color: #888;
}

/* Styling for error message inside form-group */
.form-group .error-message {
  color: red;
  font-size: 0.875rem;
  margin-top: 0.25rem;
}

:root {
  --button-color: hsla(160, 100%, 37%, 1);
  /* Original Green */
  --button-hover-color: hsla(160, 100%, 27%, 1);
  /* Darker Green */
}

button {
  background: hsla(160, 100%, 37%, 1);
  font-weight: 600;
  color: white;
  appearance: none;
  border: none;
  padding: 16px 24px;
  border-radius: 8px;
  cursor: pointer;
  margin: 12px 0;
  width: 100%;
  transition: background-color 0.3s, transform 0.2s ease-in-out;
}

button:hover {
  background: --button-hover-color;
  transform: scale(1.05);
}
</style>

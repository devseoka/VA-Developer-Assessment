<template>
  <div id="add-user" tabindex="-1" aria-hidden="true"
    class="hidden overflow-y-auto overflow-x-hidden fixed top-0 right-0 left-0 z-50 justify-center items-center w-full md:inset-0 h-[calc(100%-1rem)] max-h-full">
    <div class="relative p-4 w-full max-w-md max-h-full">
      <!-- Modal content -->
      <div class="relative bg-white rounded-lg shadow-sm">
        <!-- Modal header -->
        <div class="flex items-center justify-between p-4 md:p-5 border-b rounded-t border-gray-200">
          <h3 class="text-lg font-semibold text-assessment-primary-500">
            Create New User
          </h3>
          <button type="button"
            class="text-assessment-primary-500 bg-transparent hover:bg-assessment-primary-300 hover:text-assessment-primary-700 rounded-lg text-sm w-8 h-8 ms-auto inline-flex justify-center items-cente"
            data-modal-toggle="add-user">
            <svg class="w-3 h-3" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 14 14">
              <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6" />
            </svg>
            <span class="sr-only">Close modal</span>
          </button>
        </div>
        <!-- Modal body -->
        <form class="p-4 md:p-5">
          <div class="grid gap-4 mb-4 grid-cols-2">
            <div class="col-span-2 sm:col-span-1">
              <label for="fisrtName" class="block mb-2 text-sm font-medium text-assessment-secondary-600">First
                Name:</label>
              <input type="text" id="fisrtName"
                class="bg-gray-50 border border-gray-300 text-assessment-secondary-700 text-sm rounded-lg focus:ring-assessment-accent-500 focus:border-assessment-accent-500 block w-full  p-2.5"
                placeholder="Joe" required>
            </div>
            <div class="col-span-2 sm:col-span-1">
              <label for="lastName" class="block mb-2 text-sm font-medium text-assessment-secondary-600">Last
                Name</label>
              <input type="text" id="lastName"
                class="bg-gray-50 border border-gray-300 text-assessment-secondary-700 text-sm rounded-lg focus:ring-assessment-accent-500 focus:border-assessment-accent-500 block w-full p-2.5"
                placeholder="Smith" required>
            </div>
            <div class="col-span-2">
              <label for="idNo" class="block mb-2 text-sm font-medium text-assessment-secondary-600">Id No:</label>
              <input type="text" id="idNo"
                class="bg-gray-50 border border-gray-300 text-assessment-secondary-700 text-sm rounded-lg focus:ring-assessment-accent-500 focus:border-assessment-accent-500 block w-full p-2.5"
                placeholder="South African Id Number" required>
            </div>

          </div>
          <button type="submit"
            class="text-white inline-flex items-center bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center">
            <svg class="me-1 -ms-1 w-5 h-5" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
              <path fill-rule="evenodd"
                d="M10 5a1 1 0 011 1v3h3a1 1 0 110 2h-3v3a1 1 0 11-2 0v-3H6a1 1 0 110-2h3V6a1 1 0 011-1z"
                clip-rule="evenodd"></path>
            </svg>
            Add new product
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

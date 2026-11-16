<script setup>
import { ref, computed } from 'vue'
import BookItem from './BookItem.vue'

const props = defineProps({
  books: { type: Array, required: true },
})

defineEmits(['edit', 'delete', 'sold', 'available'])

const searchKey = ref('')

// Search runs in the browser, on the books we already have
const filteredBooks = computed(() => {
  const key = searchKey.value.trim().toLowerCase()
  return props.books.filter((b) => b.title.toLowerCase().includes(key))
})
</script>

<template>
  <section>
    <div class="filters">
      <div class="form-group">
        <label for="search">Search by title</label>
        <input
          id="search"
          v-model="searchKey"
          class="form-control"
          placeholder="Type to filter the list"
          autocomplete="off"
        />
      </div>
    </div>

    <table v-if="filteredBooks.length" class="table">
      <thead>
        <tr>
          <th>Title</th>
          <th>Status</th>
          <th class="col-actions">Actions</th>
        </tr>
      </thead>
      <tbody>
        <BookItem
          v-for="book in filteredBooks"
          :key="book.id"
          :book="book"
          @edit="$emit('edit', $event)"
          @delete="$emit('delete', $event)"
          @sold="$emit('sold', $event)"
          @available="$emit('available', $event)"
        />
      </tbody>
    </table>

    <p v-else class="empty">
      {{
        books.length === 0
          ? 'No books yet. Add your first book above.'
          : 'No books match your search.'
      }}
    </p>
  </section>
</template>
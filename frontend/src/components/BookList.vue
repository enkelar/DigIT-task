<script setup>
import { ref, computed } from 'vue'
import { Search } from 'lucide-vue-next'
import BookItem from './BookItem.vue'

const props = defineProps({
  books: { type: Array, required: true },
})

defineEmits(['edit', 'delete', 'sold', 'available'])

const searchKey = ref('')

// Search runs in the browser, on the books we already have
const filteredBooks = computed(() => {
  const key = searchKey.value.trim().toLowerCase()
  return props.books.filter(
    (b) => b.title.toLowerCase().includes(key) || b.author.toLowerCase().includes(key)
  )
})
</script>

<template>
  <section>
    <div class="search">
      <Search class="search-icon" :size="16" aria-hidden="true" />
      <label for="search" class="sr-only">Search by title or author</label>
      <input
        id="search"
        v-model="searchKey"
        class="search-input"
        placeholder="Search by title or author..."
        autocomplete="off"
      />
    </div>

    <ul v-if="filteredBooks.length" class="shelf">
      <BookItem
        v-for="book in filteredBooks"
        :key="book.id"
        :book="book"
        :number="books.indexOf(book) + 1"
        @edit="$emit('edit', $event)"
        @delete="$emit('delete', $event)"
        @sold="$emit('sold', $event)"
        @available="$emit('available', $event)"
      />
    </ul>

    <p v-else class="empty">
      {{
        books.length === 0
          ? "No books yet. Add your first book to start the shelf."
          : "No books match your search."
      }}
    </p>
  </section>
</template>

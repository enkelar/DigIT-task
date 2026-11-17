<script setup>
import { ref } from "vue";

// book = the book being edited, or null when creating a new one
const props = defineProps({
  book: { type: Object, default: null },
});

const emit = defineEmits(["save", "cancel"]);

const title = ref(props.book ? props.book.title : "");
const author = ref(props.book ? props.book.author : "");
const price = ref(props.book ? props.book.price : 0);

function submit() {
  emit("save", {
    title: title.value.trim(),
    author: author.value.trim(),
    price: Number(price.value) || 0,
  });
}
</script>

<template>
  <section class="card">
    <h2>{{ book ? "Edit book" : "Add new book" }}</h2>

    <form @submit.prevent="submit">
      <div class="form-group">
        <label for="book-title">Title</label>
        <input
          id="book-title"
          v-model="title"
          class="form-control"
          maxlength="100"
          placeholder="Enter book title"
          autocomplete="off"
        />
        <small class="hint">{{ title.length }} / 100 characters</small>
      </div>
      <div class="form-group">
        <label for="book-author">Author</label>
        <input
          id="book-author"
          v-model="author"
          class="form-control"
          maxlength="100"
          placeholder="Enter author"
          autocomplete="off"
        />
        <small class="hint">{{ author.length }} / 100 characters</small>
      </div>

      <div class="form-group">
        <label for="book-price">Price (€)</label>
        <input
          id="book-price"
          v-model="price"
          type="number"
          min="0"
          step="0.01"
          class="form-control"
        />
      </div>

      <button type="submit" class="btn btn-primary">
        {{ book ? "Save changes" : "Create book" }}
      </button>
      <button type="button" class="btn btn-default" @click="emit('cancel')">
        Cancel
      </button>
    </form>
  </section>
</template>

<script setup>
import { ref } from "vue";

// book = the book being edited, or null when creating a new one
const props = defineProps({
  book: { type: Object, default: null },
});

const emit = defineEmits(['save', 'cancel']);

const title = ref(props.book ? props.book.title : "");

function submit() {
  emit("save", title.value.trim());
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

      <button type="submit" class="btn btn-primary">
        {{ book ? "Save changes" : "Create book" }}
      </button>
      <button type="button" class="btn btn-default" @click="emit('cancel')">
        Cancel
      </button>
    </form>
  </section>
</template>

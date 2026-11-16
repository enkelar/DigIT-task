<script setup>
import { ref, onMounted } from "vue";
import {
  getBooks,
  createBook,
  updateBook,
  deleteBook,
  markBookSold,
  markBookAvailable,
} from "./api/books";
import BookForm from "./components/BookForm.vue";
import BookList from "./components/BookList.vue";

const books = ref([]);
const editingBook = ref(null); 
const showForm = ref(false); 
const error = ref("");
const loading = ref(true);

const formKey = ref(0);

const NETWORK_ERROR = "Could not reach the API. Check that it is running.";

async function loadBooks() {
  try {
    books.value = await getBooks();
  } catch {
    error.value = "Could not load books. Check that the API is running.";
  } finally {
    loading.value = false;
  }
}

function openAdd() {
  error.value = "";
  editingBook.value = null;
  formKey.value++;
  showForm.value = true;
}

function resetForm() {
  editingBook.value = null;
  formKey.value++;
  showForm.value = false;
}

function startEdit(book) {
  error.value = "";
  editingBook.value = book;
  formKey.value++;
  showForm.value = true;
  window.scrollTo({ top: 0, behavior: "smooth" });
}

async function handleSave(title) {
  error.value = "";

  if (!title) {
    error.value = "Title is required.";
    return;
  }

  const editing = editingBook.value;

  try {
    let response;
    if (editing) {
      const current = books.value.find((b) => b.id === editing.id);
      response = await updateBook({
        id: editing.id,
        title,
        isSold: current ? current.isSold : editing.isSold,
      });
    } else {
      response = await createBook({ title, isSold: false });
    }

    if (response.ok) {
      resetForm();
      await loadBooks();
    } else if (response.status === 400) {
      error.value = "Title is required and must be 100 characters or less.";
    } else if (response.status === 404) {
      error.value = "This book no longer exists.";
      resetForm();
      await loadBooks();
    } else {
      error.value = "Could not save the book.";
    }
  } catch {
    error.value = NETWORK_ERROR;
  }
}

async function handleAvailable(id) {
  await runAction(() => markBookAvailable(id), 'Could not mark the book as available.')
}

async function runAction(action, failMessage) {
  error.value = "";
  try {
    const response = await action();
    if (response.ok) {
      await loadBooks();
    } else {
      error.value = failMessage;
    }
  } catch {
    error.value = NETWORK_ERROR;
  }
}

async function handleDelete(id) {
  if (!confirm("Delete this book? This cannot be undone.")) return;
  if (editingBook.value && editingBook.value.id === id) resetForm();
  await runAction(() => deleteBook(id), "Could not delete the book.");
}

async function handleSold(id) {
  await runAction(() => markBookSold(id), "Could not mark the book as sold.");
}

onMounted(loadBooks);
</script>

<template>
  <div class="container">
    <header class="page-header">
      <div class="branding">
        <h1>Book List</h1>
      </div>
      <button v-if="!showForm" class="btn btn-primary" @click="openAdd">
        Add book
      </button>
    </header>

    <main>
      <BookForm
        v-if="showForm"
        :key="formKey"
        :book="editingBook"
        @save="handleSave"
        @cancel="resetForm"
      />

      <p v-if="error" class="alert alert-danger" role="alert">{{ error }}</p>

      <p v-if="loading" class="empty">Loading books...</p>
      <BookList
        v-else
        :books="books"
        @edit="startEdit"
        @delete="handleDelete"
        @sold="handleSold"
        @available="handleAvailable"
      />
    </main>
  </div>
</template>

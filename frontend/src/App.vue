<script setup>
import { ref, computed, onMounted } from "vue";
import { Plus } from "lucide-vue-next";
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

const subtitle = computed(() => {
  const total = books.value.length;
  if (total === 0) return "The shelf is empty.";
  const available = books.value.filter((b) => !b.isSold).length;
  const volumes = total === 1 ? "volume" : "volumes";
  return `${total} ${volumes} on the shelf, ${available} still looking for a reader.`;
});

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

async function handleSave(data) {
  error.value = "";

  if (!data.title || !data.author) {
    error.value = "Title and author are required.";
    return;
  }

  const editing = editingBook.value;

  try {
    let response;
    if (editing) {
      const current = books.value.find((b) => b.id === editing.id);
      response = await updateBook({
        id: editing.id,
        ...data,
        isSold: current ? current.isSold : editing.isSold,
      });
    } else {
      response = await createBook({ ...data, isSold: false });
    }

    if (response.ok) {
      resetForm();
      await loadBooks();
    } else if (response.status === 400) {
      error.value =
        "Title and author are required and must be 100 characters or less.";
    } else if (response.status === 409) {
      error.value = "A book with this title and author already exists.";
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
  await runAction(
    () => markBookAvailable(id),
    "Could not mark the book as available.",
  );
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
      <div class="heading">
        <h1>Book <em>Shelf</em></h1>
        <p v-if="!loading" class="subtitle">{{ subtitle }}</p>
      </div>

      <button v-if="!showForm" class="btn btn-primary add-btn" @click="openAdd">
        <Plus :size="15" aria-hidden="true" />
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

<script setup>
import { Pencil, Trash2 } from "lucide-vue-next";

defineProps({
  book: { type: Object, required: true },
});

defineEmits(["edit", "delete", "sold", "available"]);
</script>

<template>
  <tr :class="{ sold: book.isSold }">
    <td class="title">{{ book.title }}</td>
    <td>
      <span
        class="badge"
        :class="book.isSold ? 'badge-sold' : 'badge-available'"
      >
        {{ book.isSold ? "Sold" : "Available" }}
      </span>
    </td>
    <td class="col-actions">
      <button
        class="btn btn-warning btn-xs"
        title="Edit Book"
        @click="$emit('edit', book)"
      >
        <Pencil :size="14" />
      </button>
      <button
        class="btn btn-danger btn-xs"
        title="Delete Book"
        @click="$emit('delete', book.id)"
      >
        <Trash2 :size="14" />
      </button>
      <button
        v-if="!book.isSold"
        class="btn btn-success btn-xs"
        title="Mark as Sold"
        @click="$emit('sold', book.id)"
      >
        SOLD
      </button>
      <button
        v-else
        class="btn btn-default btn-xs"
        @click="$emit('available', book.id)"
      >
        AVAILABLE
      </button>
    </td>
  </tr>
</template>

<style scoped>
.sold .title {
  text-decoration: line-through;
  color: #888;
}
</style>

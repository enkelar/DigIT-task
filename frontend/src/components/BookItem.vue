<script setup>
import { computed } from 'vue'
import { Pencil, Trash2 } from 'lucide-vue-next'

const props = defineProps({
  book: { type: Object, required: true },
  number: { type: Number, default: 1 }, // position on the shelf (1, 2, 3...)
})

defineEmits(['edit', 'delete', 'sold', 'available'])

// Spine colors, picked by position.
const SPINES = ['#25503a', '#233f6a', '#7a2e2e', '#5a3a6b', '#8a5a1f', '#2f5d62']

const spineColor = computed(() => SPINES[(props.number - 1) % SPINES.length])
const paddedNumber = computed(() => String(props.number).padStart(2, '0'))
</script>

<template>
  <li class="book-card" :class="{ sold: book.isSold }">
    <span class="spine" :style="{ background: spineColor }" aria-hidden="true"></span>
    <span class="number" aria-hidden="true">{{ paddedNumber }}</span>

    <div class="info">
      <h3 class="title">{{ book.title }}</h3>
      <span class="status" :class="book.isSold ? 'status-sold' : 'status-available'">
        {{ book.isSold ? 'Sold' : 'Available' }}
      </span>
    </div>

    <div class="actions">
      <button
        class="icon-btn"
        data-tooltip="Edit book"
        aria-label="Edit book"
        @click="$emit('edit', book)"
      >
        <Pencil :size="15" />
      </button>

      <button
        class="icon-btn icon-btn-danger"
        data-tooltip="Delete book"
        aria-label="Delete book"
        @click="$emit('delete', book.id)"
      >
        <Trash2 :size="15" />
      </button>

      <button
        v-if="!book.isSold"
        class="pill-btn"
        data-tooltip="Mark this book as sold"
        @click="$emit('sold', book.id)"
      >
        Mark sold
      </button>
      <button
        v-else
        class="pill-btn"
        data-tooltip="Make this book available again"
        @click="$emit('available', book.id)"
      >
        Restock
      </button>
    </div>
  </li>
</template>
const API_URL = 'http://localhost:5107/api/books'

const JSON_HEADERS = { 'Content-Type': 'application/json' }

// GET /api/books
export async function getBooks() {
  const response = await fetch(API_URL)
  if (!response.ok) throw new Error('Failed to load books')
  return response.json()
}

// POST /api/books
export function createBook(book) {
  return fetch(API_URL, {
    method: 'POST',
    headers: JSON_HEADERS,
    body: JSON.stringify(book),
  })
}

// PUT /api/books/:id
export function updateBook(book) {
  return fetch(`${API_URL}/${book.id}`, {
    method: 'PUT',
    headers: JSON_HEADERS,
    body: JSON.stringify(book),
  })
}

// DELETE /api/books/:id
export function deleteBook(id) {
  return fetch(`${API_URL}/${id}`, { method: 'DELETE' })
}

// PATCH /api/books/:id/sold
export function markBookSold(id) {
  return fetch(`${API_URL}/${id}/sold`, { method: 'PATCH' })
}

// PATCH /api/books/:id/available 
export function markBookAvailable(id) {
  return fetch(`${API_URL}/${id}/available`, { method: 'PATCH' })
}
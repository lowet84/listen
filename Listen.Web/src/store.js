import Vue from 'vue'
import Vuex from 'vuex'
import Api from './api'

Vue.use(Vuex)

const state = {
  activePage: '-',
  books: [],
  settings: null,
  editingBook: null
}

const mutations = {
  setActivePage (state, page) {
    state.activePage = page
  },
  setEditingBook (state, book) {
    state.editingBook = book
  },
  setEditingBookCover (state, cover) {
    state.editingBook.coverImage = cover
  },
  saveSettings (state) {
    let path = state.settings.path.replace(/\\/g, '\\\\\\\\')
    let query =
      'mutation{saveSettings(path:"' +
      path +
      '" autoMatchThreshold:' +
      state.settings.autoMatchThreshold +
      '){result{autoMatchThreshold path}}}'
    Api(query)
  },
  async saveBook (state, book) {
    let saveMuation = 'mutation{editBook(' +
      `bookId:"${book.id}" ` +
      `title:"${book.title}" ` +
      `author:"${book.author}" ` +
      `imageId:"${book.coverImage.id}"` +
      '){clientMutationId}}'
    await Api(saveMuation)
  }
}

const actions = {
  async updateBooks () {
    await Api('#updateFileChanges')
    let books = await Api('#allBooks')
    Vue.set(state, 'books', books.allBooks)
    for (var index = 0; index < state.books.length; index++) {
      var element = state.books[index]
      if (element.state === 0) {
        let mutation =
          'mutation{lookupBook(id:"' +
          element.id +
          '"){result{title author id state bookState coverImage{id}}}}'
        let newElement = await Api(mutation)
        Vue.set(state.books, index, newElement.lookupBook.result)
      }
    }
  },
  async updateSettings () {
    let settings = await Api('#settings')
    state.settings = settings.settings
    if (state.settings == null) {
      state.settings = { autoMatchThreshold: 100, path: '' }
    }
  },
  async getBook (state, id) {
    let storeBook = state.state.books.find(d => d.id === id)
    if (storeBook !== undefined) {
      return storeBook
    }
    let apiBook = await Api(
      `query{book(id:"${id}"){id title author coverImage{id} path encodedPath}}`)
    return apiBook.book
  },
  async searchCovers (state, string) {
    var searchMutation =
      'mutation{searchForCovers(searchString:"' +
      `${string}` +
      '"){result{images{url id contentType}}}}'
    let searchResult = await Api(searchMutation)
    return searchResult.searchForCovers.result.images
  }
}

// getters are functions
const getters = {
}

export default new Vuex.Store({
  state,
  getters,
  actions,
  mutations
})

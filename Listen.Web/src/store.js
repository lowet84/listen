/* global __api__ */
import Vue from 'vue'
import Vuex from 'vuex'
import Api from './api'
import { getAccessToken } from './auth'

Vue.use(Vuex)

const state = {
  activePage: '-',
  backPage: undefined,
  books: [],
  settings: null,
  editingBook: null,
  user: null
}

const mutations = {
  setActivePage (state, page) {
    state.activePage = page.name
    state.backPage = page.back
  },
  setEditingBook (state, book) {
    state.editingBook = book
  },
  setEditingBookCover (state, cover) {
    state.editingBook.coverImage = cover
    let token = getAccessToken()
    state.editingBook.imageUrl =
      `${__api__}/images/${cover.id}___${token}`
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
    let token = getAccessToken()
    Vue.set(state, 'books', books.allBooks)
    for (var index = 0; index < state.books.length; index++) {
      var element = state.books[index]
      element.imageUrl = `${__api__}/images/${element.coverImage.id}___${token}`
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
    let token = getAccessToken()
    apiBook.book.imageUrl = `${__api__}/images/${apiBook.book.coverImage.id}___${token}`
    return apiBook.book
  },
  async searchCovers (state, string) {
    var searchMutation =
      'mutation{searchForCovers(searchString:"' +
      `${string}` +
      '"){result{images{url id contentType}}}}'
    let searchResult = await Api(searchMutation)
    return searchResult.searchForCovers.result.images
  },
  async addFirstUser (state, userName) {
    await Api(`mutation{addFirstUser(userName:"${userName}"){result{id}}}`)
  },
  async applyForLogin (state, userName) {
    let mutation = `mutation{applyForLogin(userName:"${userName}"){result{id}}}`
    await Api(mutation)
  },
  async setCurrentUser () {
    let result = await Api('query{myUser{userName userType}}')
    state.user = result.myUser
  }
}

// getters are functions
const getters = {
  async isFirstLogin (state) {
    let result = await Api('query{isFirstLogin{result}}')
    return result.isFirstLogin.result
  },
  async isAuthenticated (state) {
    let result = await Api('query{isAuthenticated{result}}')
    return result.isAuthenticated.result
  },
  async getApplyingUser (state) {
    let result = await Api('query{getApplyingUsername{result}}')
    return result.getApplyingUsername.result
  }
}

export default new Vuex.Store({
  state,
  getters,
  actions,
  mutations
})

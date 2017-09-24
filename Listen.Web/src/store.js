import Vue from 'vue'
import Vuex from 'vuex'
import Api from './api'

Vue.use(Vuex)

const state = {
  activePage: '-',
  books: [],
  settings: null
}

const mutations = {
  setActivePage (state, page) {
    state.activePage = page
  },
  saveSettings (state) {
    var path = state.settings.path.replace(/\\/g, '\\\\\\\\')
    var query =
      'mutation{saveSettings(path:"' +
      path +
      '" autoMatchThreshold:' +
      state.settings.autoMatchThreshold +
      '){result{autoMatchThreshold path}}}'
    console.log(query)
    Api(query)
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
        let mutation = 'mutation{lookupBook(id:"' + element.id + '"){result{title author id state bookState coverImage{id}}}}'
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

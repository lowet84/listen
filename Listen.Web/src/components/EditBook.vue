<template>
  <md-card v-if="book!==null">
    <md-card-media>
      <div class="cover-image">
        <img :src="book.imageUrl" alt="Cover image">
      </div>
    </md-card-media>

    <md-card-header>
      <md-input-container class="md-title">
        <label>Title</label>
        <md-input v-model="book.title"></md-input>
      </md-input-container>
      <md-input-container class="md-subhead">
        <label>Author</label>
        <md-input v-model="book.author"></md-input>
      </md-input-container>
    </md-card-header>

    <md-card-actions>
      <md-button class="md-accent" @click="searchForCovers">Find cover image</md-button>
      <md-button class="md-primary" @click="save">Save</md-button>
    </md-card-actions>

    <md-card-content>
      {{book.path}}
    </md-card-content>
  </md-card>
</template>

<script>
import { mapMutations, mapActions } from 'vuex'
export default {

  data () {
    return {
    }
  },

  created () {
    this.init()
    this.setActivePage({ name: 'Editing book', back: '/books' })
  },

  computed: {
    book: function () {
      return this.$store.state.editingBook
    }
  },

  methods: {
    async init () {
      if (this.$store.state.editingBook != null &&
        this.$store.state.editingBook.id === this.id) {
        return
      }
      let book = await this.getBook(this.id)
      this.setEditingBook(book)
    },
    ...mapMutations([
      'setActivePage', 'setEditingBook', 'saveBook']),
    ...mapActions(['getBook']),
    async save () {
      await this.saveBook(this.book)
    },
    async searchForCovers () {
      this.$router.push('/searchCover')
      this.isSearchingCovers = true
    }
  },

  props: ['id'],

  name: 'editBook'
}
</script>

<style>
.cover-image {
  max-height: 400px;
  max-width: 300px;
  overflow: hidden;
}
</style>

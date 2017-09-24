<template>
  <md-card>
    <md-card-media>
      <img :src="imageUrl" alt="People">
    </md-card-media>

    <md-card-header>
      <div class="md-title">Title goes here</div>
      <div class="md-subhead">Subtitle here</div>
    </md-card-header>

    <md-card-actions>
      <md-button>Action</md-button>
      <md-button>Action</md-button>
    </md-card-actions>

    <md-card-content>
      Lorem ipsum dolor sit amet, consectetur adipisicing elit. Optio itaque ea, nostrum odio. Dolores, sed accusantium quasi non, voluptas eius illo quas, saepe voluptate pariatur in deleniti minus sint. Excepturi.
    </md-card-content>
  </md-card>
</template>

<script>
/* global __api__ */
import Api from '../api'
export default {
  data () {
    return {
      book: undefined
    }
  },
  created () {
    this.init()
  },
  computed: {
    imageUrl: function () {
      if (this.book === undefined) return ''
      return `${__api__}/images/${this.book.coverImage.id}`
    }
  },
  methods: {
    async init () {
      let book = await this.getBook(this.id)
      this.book = book
    },
    async getBook (id) {
      let storeBook = this.$store.state.books.find(d => d.id === this.id)
      if (storeBook !== undefined) {
        return storeBook
      }
      let apiBook = await Api(`query{book(id:"${id}"){title author coverImage{id}}}`)
      return apiBook.book
    }
  },
  props: ['id'],
  name: 'editBook'
}
</script>

<style>

</style>

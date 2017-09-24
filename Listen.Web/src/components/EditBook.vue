<template>
  <div v-if="book!=null">
    <span>{{book.title}}</span>
    <span>{{book.author}}</span>
  </div>
</template>

<script>
import Api from '../api'
export default {
  data () {
    return {
      book: null
    }
  },
  created () {
    this.init()
  },
  methods: {
    async init () {
      let book = await this.getBook(this.id)
      this.book = book.book
    },
    async getBook (id) {
      let book = await Api(`query{book(id:"${id}"){title author}}`)
      return book
    }
  },
  props: ['id'],
  name: 'editBook'
}
</script>

<style>

</style>

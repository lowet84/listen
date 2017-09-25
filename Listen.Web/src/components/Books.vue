<template>
  <div>
    <md-list class="md-triple-line">
      <md-list-item v-for="book in $store.state.books" :key="book.id" @click="log('play')">
        <md-avatar class="md-large">
          <img :src="imageUrl(book.coverImage.id)" alt="Book">
        </md-avatar>

        <div class="md-list-text-container">
          <span>{{book.title}}</span>
          <span>{{book.author}}</span>
          <span>{{book.path}}</span>
        </div>

        <md-button class="md-icon-button md-list-action" @click="edit(book.id)">
          <md-icon class="md-primary">edit</md-icon>
        </md-button>

        <md-divider class="md-inset"></md-divider>
      </md-list-item>
    </md-list>
  </div>
</template>

<script>
/* global __api__ */
import { mapMutations, mapActions } from 'vuex'
export default {
  created () {
    this.setActivePage('Books')
    this.updateBooks()
  },
  methods: {
    ...mapMutations([
      'setActivePage']),
    ...mapActions([
      'updateBooks']),
    log (text) {
      console.log(text)
    },
    edit (id) {
      this.$router.push(`/edit/${id}`)
    },
    imageUrl (id) {
      return `${__api__}/images/${id}`
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>

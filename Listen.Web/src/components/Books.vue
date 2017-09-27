<template>
  <div>
    <md-list class="md-triple-line">
      <md-list-item v-for="book in $store.state.books" :key="book.id" @click="play">
        <md-avatar class="md-large">
          <img :src="book.imageUrl" alt="Book">
        </md-avatar>

        <div class="md-list-text-container">
          <span>{{book.title}}</span>
          <span>{{book.author}}</span>
          <span v-if="admin">{{book.path}}</span>
        </div>

        <md-button v-if="admin" class="md-icon-button md-list-action" @click="edit(book.id)">
          <md-icon class="md-primary">edit</md-icon>
        </md-button>

        <md-divider class="md-inset"></md-divider>
      </md-list-item>
    </md-list>
  </div>
</template>

<script>
import { mapMutations, mapActions } from 'vuex'
export default {
  created () {
    this.setActivePage({ name: 'Books' })
    this.updateBooks()
  },
  computed: {
    admin: function () {
      let user = this.$store.state.user
      if (user === null || user.userType !== 1) {
        return false
      }
      return true
    }
  },
  methods: {
    ...mapMutations([
      'setActivePage']),
    ...mapActions([
      'updateBooks', 'getImageUrl']),
    play () {
      console.log('play')
    },
    edit (id) {
      this.$router.push(`/edit/${id}`)
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>

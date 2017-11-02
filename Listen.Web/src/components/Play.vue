<template>
  <md-card v-if="book!==null">
    <md-card-media-cover>
      <md-card-media>
        <div class="cover-image">
          <img :src="book.imageUrl" alt="Cover image">
        </div>
      </md-card-media>
    </md-card-media-cover>
    <!-- <md-card-area>
      <md-card-header>
        <div class="md-title">{{book.title}}</div>
        <div class="md-subhead">{{book.author}}</div>
      </md-card-header>
    </md-card-area> -->
    <md-progress md-progress="50" class="progress md-accent"></md-progress>

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
    this.setActivePage({ name: this.book.title, back: '/books' })
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

<style scoped>
.cover-image {
  max-height: 400px;
  max-width: 300px;
  overflow: hidden;
}

.progress {
  height: 30px;
  max-width: 300px;
}

.main {
  margin: 0px
}
</style>

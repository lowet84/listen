<template>
  <div v-if="covers!==null">
    <md-card v-for="cover in covers" :key="cover.id" class="card">
      <md-card-media>
        <div class="cover-image">
          <img :src="cover.url" />
        </div>
      </md-card-media>

      <md-card-actions>
        <md-button class="md-accent" @click="select(cover)">Select image</md-button>
      </md-card-actions>
    </md-card>
  </div>
</template>

<script>
import { mapActions, mapMutations } from 'vuex'
export default {
  data () {
    return {
      covers: null
    }
  },
  name: 'coverSearch',
  created () {
    this.setActivePage({ name: 'Find cover images', back: `/edit/${this.book.id}` })
    if (this.$store.state.editingBook == null) {
      this.$router.push('/')
    }
    this.search()
  },
  computed: {
    book: function () {
      return this.$store.state.editingBook
    }
  },
  methods: {
    ...mapActions(['searchCovers']),
    ...mapMutations(['setEditingBookCover', 'setActivePage']),
    async search () {
      let covers = await this.searchCovers(
        `${this.book.title} ${this.book.author}`)
      this.covers = covers
    },
    async select (cover) {
      this.setEditingBookCover(cover)
      this.$router.push(`/edit/${this.book.id}`)
    }
  }
}
</script>

<style>
.card {
  margin-bottom: 15px
}
.cover-image {
  max-height: 400px;
  overflow: hidden;
}
</style>

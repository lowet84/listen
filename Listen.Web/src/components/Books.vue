<template>
  <div>
    <md-list>
      <md-list-item v-for="(books,name) in authors" :key="name">
        <md-icon>person_outline</md-icon>
        <span>{{name}} ({{books.length}})</span>

        <md-list-expand>
          <md-list>
            <md-list-item class="md-inset" v-for="book in books" :key="book.id">
              <md-card class="card">
                <md-card-media>
                  <div class="cover-image">
                    <img :src="book.imageUrl" @click="play(book.id)" />
                  </div>
                </md-card-media>

                <md-card-actions>
                  <md-button class="md-accent" @click="edit(book.id)">Edit</md-button>
                  <md-button class="md-primary" @click="play(book.id)">Play</md-button>
                </md-card-actions>
              </md-card>

            </md-list-item>
          </md-list>
        </md-list-expand>
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
    },
    loggedIn: function () {
      let user = this.$store.state.user
      if (user === null || (user.userType !== 1 && user.userType !== 0)) {
        return false
      }
      return true
    },
    authors: function () {
      var books = this.$store.state.books
      let authors = this.groupArrayBy(books, 'author')
      return authors
    }
  },
  methods: {
    ...mapMutations([
      'setActivePage']),
    ...mapActions([
      'updateBooks', 'getImageUrl']),
    edit (id) {
      this.$router.push(`/edit/${id}`)
    },
    groupArrayBy (array, prop) {
      return array.reduce(function (groups, item) {
        var val = item[prop]
        groups[val] = groups[val] || []
        groups[val].push(item)
        return groups
      }, {})
    },
    play (id) {
      this.$router.push(`/play/${id}`)
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.cover-image {
  max-height: 400px;
  overflow: hidden;
}

.card {
  margin-bottom: 15px
}
</style>

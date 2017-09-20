<template>
  <div>
    <div v-for="book in books" :key="book.id">
      <img class="cover-image" :src="'http://localhost:7000/images/' + book.coverImage.id" />
    </div>
  </div>
</template>

<script>
import Vue from 'vue'
export default {
  name: 'hello',
  data () {
    return {
      books: []
    }
  },
  created () {
    this.update()
  },
  methods: {
    async update () {
      await window.api('mutation{updateFileChanges{result{clientMutationId}}}')
      let books = await window.api('query{allBooks{title author id state bookState coverImage{id}}}')
      this.books = books.allBooks
      for (var index = 0; index < this.books.length; index++) {
        var element = this.books[index]
        if (element.state === 0) {
          let mutation = 'mutation{lookupBook(id:"' + element.id + '"){result{title author id state bookState coverImage{id}}}}'
          let newElement = await window.api(mutation)
          Vue.set(this.books, index, newElement.lookupBook.result)
        }
      }
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.cover-image{
  max-height: 400px;
  max-width: 400px;
}
h1,
h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}
</style>

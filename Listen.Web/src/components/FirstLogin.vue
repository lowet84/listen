<template>
  <md-card>
    <md-card-media>
      <div>
        <!-- <img :src="imageUrl" alt="People"> -->
      </div>
    </md-card-media>

    <md-card-header>
      <div class="md-title">Welcome to Listen!</div>
      <div class="md-subhead">Please create an admin account</div>
    </md-card-header>

    <md-card-content>
      <md-input-container>
        <label>User name</label>
        <md-input v-model="userName"></md-input>
      </md-input-container>
    </md-card-content>

    <md-card-actions>
      <md-button class="md-primary" @click="add">Add account</md-button>
    </md-card-actions>
  </md-card>
</template>

<script>
import { mapMutations, mapActions } from 'vuex'

export default {
  data () {
    return {
      userName: ''
    }
  },
  name: 'firstLogin',
  created () {
    this.setActivePage({ name: 'First login' })
  },
  methods: {
    ...mapMutations([
      'setActivePage']),
    ...mapActions(['addFirstUser']),
    async add () {
      if (this.userName === '') {
        return
      }

      await this.addFirstUser(this.userName)
    }
  }
}
</script>

<style>

</style>

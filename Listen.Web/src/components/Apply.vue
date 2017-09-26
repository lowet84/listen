<template>
  <md-card>
    <md-card-media>
      <div>
        <!-- <img :src="imageUrl" alt="People"> -->
      </div>
    </md-card-media>

    <md-card-header>
      <div class="md-title">Welcome to Listen!</div>
      <div class="md-subhead">Apply for an account</div>
    </md-card-header>

    <md-card-content>
      <md-input-container>
        <label>User name</label>
        <md-input v-model="userName"></md-input>
      </md-input-container>
    </md-card-content>

    <md-card-actions>
      <md-button class="md-primary" @click="save">Save application</md-button>
    </md-card-actions>
  </md-card>
</template>

<script>
import { mapMutations, mapGetters, mapActions } from 'vuex'

export default {
  data () {
    return {
      userName: ''
    }
  },
  name: 'apply',
  created () {
    this.setActivePage({ name: 'Apply for login' })
    this.init()
  },
  methods: {
    ...mapMutations([
      'setActivePage']),
    ...mapGetters(['getApplyingUser']),
    ...mapActions(['applyForLogin']),
    async save () {
      await this.applyForLogin(this.userName)
    },
    async init () {
      let userName = await this.getApplyingUser()
      this.userName = userName
    }
  }
}
</script>

<style>

</style>

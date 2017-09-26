<template>
  <div id="app">
    <div>
      <listen-menu />
      <router-view class="main"></router-view>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { loginIfNeeded } from './auth'
export default {
  name: 'app',
  created () {
    loginIfNeeded(this.$route.path)
    this.handleAuthentication()
  },
  methods: {
    async handleAuthentication () {
      let firstLogin = await this.isFirstLogin()
      if (firstLogin) {
        this.$router.push('/firstLogin')
      }
      let authenticated = await this.isAuthenticated()
      if (!authenticated) {
        this.$router.push('/apply')
      }
    },
    ...mapGetters(['isFirstLogin', 'isAuthenticated'])
  }
}
</script>

<style>
.main {
  margin: 10px;
}
</style>

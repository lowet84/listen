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
    this.handleFirstLogin()
  },
  methods: {
    async handleFirstLogin () {
      let firstLogin = await this.isFirstLogin()
      if (firstLogin) {
        this.$router.push('/firstLogin')
      }
    },
    ...mapGetters(['isFirstLogin'])
  }
}
</script>

<style>
.main {
  margin: 10px;
}
</style>

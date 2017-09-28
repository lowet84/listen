<template>
  <div id="app">
    <div>
      <listen-menu />
      <router-view class="main"></router-view>
    </div>
  </div>
</template>

<script>
import { mapActions } from 'vuex'
export default {
  name: 'app',
  created () {
    this.setCurrentUser()
  },
  methods: {
    ...mapActions(['setCurrentUser'])
  }
}
</script>

<style>
.main {
  margin: 10px;
}
</style>

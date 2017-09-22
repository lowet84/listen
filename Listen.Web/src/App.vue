<template>
  <div id="app">
    <div>
      <listen-menu />
      <router-view class="main"></router-view>
    </div>
  </div>
</template>

<script>
import { loginIfNeeded } from './auth'
export default {
  name: 'app',
  created () {
    loginIfNeeded(this.$route.path)
  }
}
</script>

<style>
.main {
  margin-left: 10px;
}
</style>

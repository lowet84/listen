<template>
  <div class="phone-viewport">

    <md-toolbar>
      <md-button v-if="!$store.state.backPage" class="md-icon-button" @click="toggleLeftSidenav">
        <md-icon>menu</md-icon>
      </md-button>
      <md-button v-if="$store.state.backPage" class="md-icon-button" @click="back">
        <md-icon>arrow_back</md-icon>
      </md-button>
      <h2 class="md-title" style="flex: 1">{{$store.state.activePage}}</h2>
    </md-toolbar>

    <md-sidenav class="md-left" ref="leftSidenav">
      <md-toolbar class="md-large">
        <div class="md-toolbar-container">
          <h3 class="md-title">Listen!</h3>
        </div>
      </md-toolbar>

      <md-list>
        <md-list-item @click="goto('')">
          <md-icon>view_list</md-icon>
          <span>Books</span>
        </md-list-item>

        <md-list-item @click="goto('settings')">
          <md-icon>settings</md-icon>
          <span>Settings</span>
        </md-list-item>

        <md-list-item @click="handleLogout">
          <md-icon>exit_to_app</md-icon>
          <span>Log out</span>
        </md-list-item>
      </md-list>
    </md-sidenav>

  </div>
</template>

<script>
import { logout } from '../auth'
export default {
  data () {
    return {
    }
  },
  methods: {
    goto (target) {
      this.$refs.leftSidenav.close()
      this.$router.push('/' + target)
    },
    toggleLeftSidenav () {
      this.$refs.leftSidenav.toggle()
    },
    back () {
      history.back()
    },
    handleLogout () {
      logout()
      this.$router.push('/')
      location.reload()
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>

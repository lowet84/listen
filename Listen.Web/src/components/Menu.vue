<template>
  <div class="phone-viewport">

    <md-toolbar>
      <md-button v-if="$store.state.backPage === undefined" class="md-icon-button" @click="toggleLeftSidenav">
        <md-icon>menu</md-icon>
      </md-button>
      <md-button v-if="$store.state.backPage !== undefined" class="md-icon-button" @click="back">
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
        <md-list-item v-if="loggedIn" @click="goto('books')">
          <md-icon>view_list</md-icon>
          <span>Books</span>
        </md-list-item>

        <md-list-item v-if="admin" @click="goto('users')">
          <md-icon>supervisor_account</md-icon>
          <span>Users</span>
        </md-list-item>

        <md-list-item v-if="admin" @click="goto('settings')">
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
import { logout, login } from '../auth'
export default {
  data () {
    return {
    }
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
      this.$router.push(this.$store.state.backPage)
    },
    async handleLogout () {
      await logout()
      await login()
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>

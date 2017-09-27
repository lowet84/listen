<template>
  <div class="phone-viewport" v-if="$store.state.allUsers != null">
    <md-list>
      <md-list-item>
        <md-icon>hourglass_empty</md-icon>
        <span>{{pendingText}}</span>

        <md-list-expand>
          <md-list>
            <md-list-item class="md-inset" v-for="user in pending" :key="user.id">
              <div>{{user.userName}}</div>
              <md-button class="md-warn" @click="handleReject(user)">Reject</md-button>
              <md-button class="md-primary" @click="handleApprove(user)">Approve</md-button>
            </md-list-item>
          </md-list>
        </md-list-expand>
      </md-list-item>

      <md-list-item>
        <md-icon>account_circle</md-icon>
        <span>{{activeText}}</span>

        <md-list-expand>
          <md-list>
            <md-list-item class="md-inset" v-for="user in active" :key="user.id">
              <div>{{user.userName}}{{adminText(user.userType)}}</div>
              <md-button class="md-warn" @click="handleReject(user)">Reject</md-button>
              <md-button class="md-primary" @click="handlePromote(user)">{{promoteText(user.userType)}}</md-button>
            </md-list-item>
          </md-list>
        </md-list-expand>
      </md-list-item>

      <md-list-item>
        <md-icon>error</md-icon>
        <span>{{rejectedText}}</span>

        <md-list-expand>
          <md-list>
            <md-list-item class="md-inset" v-for="user in rejected" :key="user.id">
              <div>{{user.userName}}</div>
              <md-button class="md-primary" @click="handleApprove(user)">Approve</md-button>
            </md-list-item>
          </md-list>
        </md-list-expand>
      </md-list-item>
    </md-list>

    <md-dialog-confirm :md-title="confirm.title" :md-content-html="confirm.contentHtml" md-ok-text="OK" md-cancel-text="Cancel" @close="onClose" ref="dialog">
    </md-dialog-confirm>

    <md-dialog-alert md-content="You cannot edit yourself" md-ok-text="OK" ref="selfRejection">
    </md-dialog-alert>
  </div>
</template>

<script>
import { mapMutations, mapActions } from 'vuex'
export default {
  data () {
    return {
      confirm: { title: '-', contentHtml: '-', ok: '-', cancel: '-', type: 0 }
    }
  },
  created () {
    this.setActivePage({ name: 'Users' })
    this.getUsers()
  },
  computed: {
    pending: function () {
      return this.$store.state.allUsers.filter(d => d.userType === 2)
    },
    active: function () {
      return this.$store.state.allUsers.filter(d => d.userType === 0 || d.userType === 1)
    },
    rejected: function () {
      return this.$store.state.allUsers.filter(d => d.userType === 3)
    },
    pendingText: function () {
      return `Pending(${this.pending.length})`
    },
    activeText: function () {
      return `Active(${this.active.length})`
    },
    rejectedText: function () {
      return `Rejected(${this.rejected.length})`
    }
  },
  methods: {
    ...mapMutations([
      'setActivePage']),
    ...mapActions(['getUsers', 'approveUser', 'changeAdminStatus', 'rejectUser']),
    promoteText (userType) {
      if (userType === 0) return 'Promote'
      return 'Demote'
    },
    adminText (type) {
      if (type === 1) return '(+)'
      return ''
    },
    handleApprove (user) {
      this.confirm = {
        title: `Approve user ${user.userName}?`,
        contentHtml: `Are you sure that you want to approve the user: ${user.userName}`,
        type: 0,
        user: user
      }
      this.$refs['dialog'].open()
    },
    handleReject (user) {
      if (user.id === this.$store.state.user.id) {
        this.$refs['selfRejection'].open()
      } else {
        this.confirm = {
          title: `Reject user ${user.userName}?`,
          contentHtml: `Are you sure that you want to reject the user: ${user.userName}`,
          type: 1,
          user: user
        }
        this.$refs['dialog'].open()
      }
    },
    handlePromote (user) {
      if (user.id === this.$store.state.user.id) {
        this.$refs['selfRejection'].open()
      } else {
        let action = 'demote'
        if (user.userType === 0) {
          action = 'promote'
        }
        this.confirm = {
          title: `${this.promoteText(user.userType)} user ${user.userName}?`,
          contentHtml: `Are you sure that you want to ${action} the user: ${user.userName}`,
          type: 2,
          user: user
        }
        this.$refs['dialog'].open()
      }
    },
    onClose (type) {
      if (type === 'ok') {
        if (this.confirm.type === 0) {
          this.approveUser(this.confirm.user)
        } else if (this.confirm.type === 1) {
          this.rejectUser(this.confirm.user)
        } else if (this.confirm.type === 2) {
          this.changeAdminStatus(this.confirm.user)
        }
      }
    }
  }
}
</script>

<style scoped>

</style>

<template>
  <div v-if="settings!=null">
    <form novalidate @submit.stop.prevent="submit">
      <md-input-container>
        <label>Path</label>
        <md-input v-model="settings.path" v-on:change="startSaveTimer"></md-input>
      </md-input-container>

      <md-input-container>
        <label>Automatch threshold</label>
        <md-input type="number" v-model="settings.autoMatchThreshold" v-on:change="startSaveTimer"></md-input>
      </md-input-container>
    </form>
  </div>
</template>

<script>
import { mapActions, mapMutations, mapState } from 'vuex'
export default {
  data () {
    return {
      timer: null
    }
  },
  created () {
    this.setActivePage('Settings')
    this.updateSettings()
  },
  computed: {
    ...mapState(['settings'])
  },
  methods: {
    ...mapMutations([
      'setActivePage', 'saveSettings']),
    ...mapActions([
      'updateSettings']),
    startSaveTimer () {
      if (this.timer != null) {
        clearTimeout(this.timer)
      }
      this.timer = setTimeout(() => {
        this.saveSettings()
      }, 800)
    }
  }
}
</script>

<style scoped>

</style>

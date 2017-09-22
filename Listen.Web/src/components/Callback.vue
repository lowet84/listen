<template>

</template>
<script>

import { setIdToken, setAccessToken } from '../auth'

export default {
  name: '',
  mounted () {
    console.log('callback')
    this.$nextTick(() => {
      setAccessToken()
      setIdToken()
      window.location.href = '/#'
    })
  }
}
</script>
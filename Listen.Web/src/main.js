// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import Api from './api'
import router from './router'
import store from './store'
import { setAccessToken, setIdToken, login } from './auth'

// Components
import Menu from '@/components/Menu'

Vue.component('listen-menu', Menu)

import 'vue-material/dist/vue-material.css'

var VueMaterial = require('vue-material')

Vue.use(VueMaterial)

Vue.config.productionTip = false

Vue.material.registerTheme({
  default: {
    primary: 'blue',
    accent: 'red'
  },
  green: {
    primary: 'green',
    accent: 'pink'
  },
  orange: {
    primary: 'orange',
    accent: 'green'
  }
})

let init = async function () {
  if (window.location.hash.startsWith('#/access_token')) {
    await setAccessToken()
    await setIdToken()
    window.location.href = '/'
  }

  let loginStatus = await Api('query{getLoginStatus}')
  console.log(loginStatus)
  if (loginStatus.getLoginStatus === 'NOT_LOGGED_IN') {
    login()
  }
  if (loginStatus.getLoginStatus === 'OK') {
    window.location.hash = '/books'
  } else if (loginStatus.getLoginStatus === 'APPLY') {
    window.location.hash = '/apply'
  } else if (loginStatus.getLoginStatus === 'REJECTED') {
    window.location.hash = '/apply'
  } else if (loginStatus.getLoginStatus === 'FIRST_LOGIN') {
    window.location.hash = '/firstLogin'
  }

  if (loginStatus.getLoginStatus !== 'ERROR') {
    /* eslint-disable no-new */
    new Vue({
      el: '#app',
      router,
      store,
      template: '<App/>',
      components: { App }
    })
  }
}

init()

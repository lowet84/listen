// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import axios from 'axios'
import VueAxios from 'vue-axios'

Vue.use(VueAxios, axios)
Vue.config.productionTip = false

window.api = async function (query) {
  var escapedQuery = query.replace(/"/g, '\\"')
  let queryString = '{query:"' + escapedQuery + '"}'
  let ret = await axios.post('http://localhost:7000/api/', queryString)
  return ret.data.data
}

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  template: '<App/>',
  components: { App }
})

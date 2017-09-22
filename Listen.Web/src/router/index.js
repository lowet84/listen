import Vue from 'vue'
import Router from 'vue-router'
import Books from '@/components/Books'
import Settings from '@/components/Settings'
import Login from '@/components/Login'
import Callback from '@/components/Callback'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Books',
      component: Books
    },
    {
      path: '/settings',
      name: 'Settings',
      component: Settings
    },
    {
      path: '/login',
      name: 'Login',
      component: Login
    },
    {
      path: '/callback',
      name: 'Callback',
      component: Callback
    }
  ]
})

import Vue from 'vue'
import Router from 'vue-router'
import Books from '@/components/Books'
import Settings from '@/components/Settings'
import EditBook from '@/components/EditBook'
import CoverSearch from '@/components/CoverSearch'
import FirstLogin from '@/components/FirstLogin'
import Apply from '@/components/Apply'
import Users from '@/components/Users'
import Play from '@/components/Play'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/books',
      name: 'Books',
      component: Books
    },
    {
      path: '/settings',
      name: 'Settings',
      component: Settings
    },
    {
      path: '/edit/:id',
      name: 'EditBook',
      component: EditBook,
      props: true
    },
    {
      path: '/searchCover',
      name: 'CoverSearch',
      component: CoverSearch
    },
    {
      path: '/firstLogin',
      name: 'FirstLogin',
      component: FirstLogin
    },
    {
      path: '/apply',
      name: 'Apply',
      component: Apply
    },
    {
      path: '/users',
      name: 'Users',
      component: Users
    },
    {
      path: '/play/:id',
      name: 'Play',
      component: Play,
      props: true
    }
  ]
})

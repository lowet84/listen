import Vue from 'vue'
import Router from 'vue-router'
import Books from '@/components/Books'
import Settings from '@/components/Settings'
import EditBook from '@/components/EditBook'

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
      path: '/edit/:id',
      name: 'EditBook',
      component: EditBook,
      props: true
    }
  ]
})

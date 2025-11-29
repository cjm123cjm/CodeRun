import { createRouter, createWebHistory } from 'vue-router'
import VueCookies from 'vue-cookies'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/Login.vue'),
    },
    {
      path: '/',
      name: 'layout',
      component: () => import('@/views/Layout.vue'),
      children: [
        {
          path: '/home',
          name: '首页',
          component: () => import('@/views/home/Home.vue'),
        },
        {
          path: '/setting/menu',
          name: '菜单管理',
          component: () => import('@/views/setting/MenuList.vue'),
        },
      ],
    },
  ],
})

router.beforeEach((to, from, next) => {
  const token = VueCookies.get('token')
  if (!token && to.path != '/login') {
    router.push('/login')
  }
  next()
})

export default router

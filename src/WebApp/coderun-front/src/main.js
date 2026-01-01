import './assets/base.scss'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

//引入ElementPlus
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
//引入自定义图标库
import '@/assets/icon/iconfont.css'
//dialog组件
import Dialog from '@/components/Dialog.vue'
import Table from '@/components/Table.vue'
import Cover from '@/components/Cover.vue'
import CoverUpload from '@/components/CoverUpload.vue'

import Request from '@/utils/Request.js'
import Message from '@/utils/Message.js'
import VueCookies from 'vue-cookies'
import Confirm from '@/utils/Confirm'
import Verify from '@/utils/Verify'
import PermissionCodes from '@/utils/PermissionCode'

const app = createApp(App)

app.use(router)
app.use(ElementPlus)

app.component('Dialog', Dialog)
app.component('Table', Table)
app.component('Cover', Cover)
app.component('CoverUpload', CoverUpload)

app.config.globalProperties.Request = Request
app.config.globalProperties.Message = Message
app.config.globalProperties.VueCookies = VueCookies
app.config.globalProperties.Confirm = Confirm
app.config.globalProperties.Verify = Verify
app.config.globalProperties.PermissionCodes = PermissionCodes
app.config.globalProperties.globalInfo = {
  avatarUrl: '/api/file/getAvatar',
  imageUrl: '/userUploadFile/',
}

//自定义指令,权限
app.directive('has', {
  mounted: (el, binding, vnode) => {
    const userInfo = JSON.parse(sessionStorage.getItem('userInfo'))
    let permissionCodes = userInfo.permissionCodes
    permissionCodes = !permissionCodes ? [] : permissionCodes
    if (!permissionCodes.includes(binding.value)) {
      el.parentNode.removeChild(el)
    }
  },
})
app.mount('#app')

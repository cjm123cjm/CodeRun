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

import Request from '@/utils/Request.js'
import Message from '@/utils/Message.js'
import VueCookies from 'vue-cookies'
import Confirm from '@/utils/Confirm'
import Verify from './utils/Verify'

const app = createApp(App)

app.use(router)
app.use(ElementPlus)

app.component('Dialog', Dialog)

app.config.globalProperties.Request = Request
app.config.globalProperties.Message = Message
app.config.globalProperties.VueCookies = VueCookies
app.config.globalProperties.Confirm = Confirm
app.config.globalProperties.Verify = Verify

app.mount('#app')

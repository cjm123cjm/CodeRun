import './assets/base.scss'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

//引入ElementPlus
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
//引入自定义图标库
import '@/assets/icon/iconfont.css'

import Request from '@/utils/Request.js'
import Message from '@/utils/Message.js'
import VueCookies from 'vue-cookies'

const app = createApp(App)

app.use(router)
app.use(ElementPlus)

app.config.globalProperties.Request = Request
app.config.globalProperties.Message = Message
app.config.globalProperties.VueCookies = VueCookies

app.mount('#app')

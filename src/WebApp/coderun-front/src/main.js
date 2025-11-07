import './assets/base.scss'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

//引入ElementPlus
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'

const app = createApp(App)

app.use(router)
app.use(ElementPlus)

app.mount('#app')

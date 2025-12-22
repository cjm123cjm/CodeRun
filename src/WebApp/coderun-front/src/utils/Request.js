import axios from 'axios'

import { ElLoading } from 'element-plus'
import router from '@/router'
import Message from '@/utils/Message'
import VueCookies from 'vue-cookies'

const contentTypeForm = 'application/x-www-form-urlencoded;charset=UTF-8'
const contentTypeJson = 'application/json;charset=UTF-8'

let loading = null
const instance = axios.create({
  baseURL: '/api',
  timeout: 10 * 1000,
})

//请求前拦截器
instance.interceptors.request.use(
  (config) => {
    if (config.showLoading) {
      loading = ElLoading.service({
        lock: true,
        text: '加载中......',
        background: 'rgba(0,0,0,0.7)',
      })
    }
    const token = VueCookies.get('token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }

    return config
  },
  (error) => {
    if (error.config && error.config.showLoading && loading) {
      loading.close()
    }
    Message.error('请求发送失败!')
    return Promise.reject('请求发送失败!')
  },
)

//请求后拦截器
instance.interceptors.response.use(
  (response) => {
    const { showLoading, errorCallback, showError = true } = response.config
    if (showLoading && loading) {
      loading.close()
    }
    const responseData = response.data
    if (responseData.code == 200) {
      if (!responseData.isSuccess) {
        return Promise.reject({ showError: showError, msg: responseData.message })
      } else {
        return responseData
      }
    } else if (responseData.code == 403) {
      return Promise.reject({ showError: showError, msg: responseData.message })
    } else if (responseData.code == 401) {
      setTimeout(() => {
        router.push('/login')
      }, 2000)
      return Promise.reject({ showError: true, msg: '登录超时' })
    } else {
      if (errorCallback) {
        errorCallback(responseData.message)
      }
      return Promise.reject({ showError: showError, msg: responseData.message })
    }
  },
  (error) => {
    if (error.config.showLoading && loading) {
      loading.close()
    }
    if (error.status == 401) {
      router.push('/login')
      return
    } else if (error.status == 403) {
      return Promise.reject({ showError: showError, msg: error.response.data.message })
    }
    return Promise.reject({ showError: true, msg: '网络异常!' })
  },
)

const request = (config) => {
  const { url, method, params, dataType, showLoading = true } = config
  let contentType = contentTypeForm
  let data = null

  // 根据 dataType 处理数据
  if (dataType === 'json') {
    contentType = contentTypeJson
    data = JSON.stringify(params || {})
  } else {
    // FormData 格式
    const formData = new FormData()
    if (params) {
      for (const key in params) {
        if (params[key] !== undefined && params[key] !== null) {
          formData.append(key, params[key])
        }
      }
    }
    data = formData
  }

  let headers = {
    'Content-Type': contentType,
    'X-Requested-With': 'XMLHttpRequest',
  }

  // 统一请求配置
  const requestConfig = {
    url,
    method: method.toLowerCase(),
    headers,
    showLoading,
    errorCallback: config.errorCallback,
    showError: config.showError,
  }

  // 根据请求方法设置数据位置
  if (method.toLowerCase() === 'get') {
    // GET 请求参数放在 params 中（URL查询参数）
    requestConfig.params = params
  } else {
    // POST、PUT、DELETE 等请求参数放在 data 中（请求体）
    requestConfig.data = data
  }

  return instance.request(requestConfig).catch((error) => {
    console.log(error)
    if (error.showError) {
      Message.error(error.msg)
    }
    return null
  })
}

export default request

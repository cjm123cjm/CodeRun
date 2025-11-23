<template>
  <div class="login-body">
    <div class="bg"></div>
    <div class="login-panel">
      <el-form
        class="login-register"
        :model="formData"
        ref="formDataRef"
        :rules="rules"
        @submit.prevent
      >
        <div class="login-title">CodeRun管理后台</div>
        <el-form-item prop="userName">
          <el-input v-model="formData.userName" size="large" clearable placeholder="请输入用户名">
            <template #prefix>
              <span class="iconfont icon-shouji1"></span>
            </template>
          </el-input>
        </el-form-item>

        <el-form-item prop="password">
          <el-input
            v-model="formData.password"
            type="password"
            show-password
            size="large"
            clearable
            placeholder="请输入密码"
          >
            <template #prefix>
              <span class="iconfont icon-mima"></span>
            </template>
          </el-input>
        </el-form-item>

        <el-form-item prop="code">
          <div class="check-code-panel">
            <el-input v-model="formData.code" size="large" clearable placeholder="请输入验证码">
              <template #prefix>
                <span class="iconfont icon-yanzhengyanzhengma"></span>
              </template>
            </el-input>
            <img class="check-code" :src="checkCodeUrl" @click="changeCheckCode" />
          </div>
        </el-form-item>
        <el-form-item>
          <div class="remember-panel"></div>
          <el-checkbox v-model="formData.rememberMe">记住我</el-checkbox>
        </el-form-item>
        <el-form-item>
          <el-button type="primary" class="op-btn" @click="login">登录</el-button>
        </el-form-item>
      </el-form>
    </div>
  </div>
</template>

<script setup>
import { nextTick, onMounted, ref, getCurrentInstance } from 'vue'
import md5 from 'js-md5'
import { useRouter } from 'vue-router'
const { proxy } = getCurrentInstance()
const router = useRouter()

const api = {
  checkCode: '/Account/CheckCode',
  login: '/Account/Login',
}

//验证码
const checkCodeUrl = ref('')
const changeCheckCode = async () => {
  const result = await proxy.Request({
    url: api.checkCode,
    method: 'get',
  })
  if (!result) return

  checkCodeUrl.value = `data:image/png;base64,${result.result.image}`
  localStorage.setItem('codeKey', result.result.codeKey)
}

const formData = ref({})
const formDataRef = ref()
const rules = {
  userName: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  password: [{ required: true, message: '请输入密码', trigger: 'blur' }],
  code: [{ required: true, message: '请输入验证码', trigger: 'blur' }],
}
//登录
const login = () => {
  formDataRef.value.validate(async (valid) => {
    if (!valid) return
    //调用登录接口
    let params = {}
    Object.assign(params, formData.value)

    let cookies = proxy.VueCookies.get('loginInfo')
    let password = cookies == null ? null : cookies.password
    if (params.password !== password) {
      params.password = md5(params.password)
    }

    params.codeKey = localStorage.getItem('codeKey')

    const result = await proxy.Request({
      url: api.login,
      method: 'post',
      params: params,
      errorCallback: () => {
        changeCheckCode()
      },
      dataType: 'json',
    })

    if (!result) {
      changeCheckCode()
      return
    }

    if (params.rememberMe) {
      let loginInfo = {
        userName: params.userName,
        password: params.password,
        rememberMe: params.rememberMe,
      }
      proxy.VueCookies.set('loginInfo', loginInfo, '7d')
    } else {
      proxy.VueCookies.remove('loginInfo')
    }

    //保存用户信息
    proxy.VueCookies.set('token', result.result.token, '7d')
    sessionStorage.setItem('userInfo', JSON.stringify(result.result))

    //跳转页面
    let firstMenu = result.result.menus[0]
    if (firstMenu && firstMenu.childMenu.length > 0) {
      firstMenu = firstMenu.childMenu[0]
    }
    proxy.Message.success('登录成功！')
    router.push(firstMenu.menuUrl)
  })
}

//初始化
const init = () => {
  nextTick(() => {
    changeCheckCode()
    formDataRef.value.resetFields()
    formData.value = {}
    const loginInfo = proxy.VueCookies.get('loginInfo')
    if (loginInfo) {
      formData.value = loginInfo
    }
  })
}

onMounted(() => {
  init()
})
</script>

<style lang="scss" scoped>
.login-body {
  height: calc(100vh);
  background: url('@/assets/bg_image.jpg');
  background-size: 100% 100%;
  background-repeat: no-repeat;
  display: flex;
  .bg {
    flex: 1;
    background-size: cover;
    background-position: center;
    background-size: 800px;
    background-repeat: no-repeat;
    background-image: url('@/assets/login.png');
  }
  .login-panel {
    width: 430px;
    margin-right: 15%;
    margin-top: calc((100vh - 500px) / 2);
    .login-register {
      padding: 25px;
      background: #fff;
      border-radius: 5px;
      .login-title {
        text-align: center;
        font-size: 18px;
        font-weight: bold;
        margin-bottom: 20px;
      }
      .send-email-panel {
        display: flex;
        width: 100%;
        justify-content: space-between;
        .send-mail-btn {
          margin-left: 5px;
        }
      }
      .remember-panel {
        width: 100%;
      }
      .no-account {
        width: 100%;
        display: flex;
        justify-content: space-between;
      }
      .op-btn {
        width: 100%;
      }
    }
  }

  .check-code-panel {
    width: 100%;
    display: flex;
    .check-code {
      margin-left: 5px;
      cursor: pointer;
    }
  }
}
</style>

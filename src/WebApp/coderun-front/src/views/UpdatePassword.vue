<template>
  <Dialog
    :show="dialogConfig.show"
    :title="dialogConfig.title"
    width="400px"
    :showCancel="true"
    :buttons="dialogConfig.buttons"
    @close="dialogConfig.show = false"
  >
    <el-form :model="formData" :rules="rules" ref="formDataRef" label-width="80px" @submit.prevent>
      <!--密码-->
      <el-form-item label="原始密码" prop="password">
        <el-input
          v-model.trim="formData.password"
          size="large"
          type="password"
          placeholder="请输入原始密码"
        >
          <template #prefix>
            <span class="iconfont icon-mima"></span>
          </template>
        </el-input>
      </el-form-item>

      <!--新密码-->
      <el-form-item label="新密码" prop="newPassword">
        <el-input
          v-model.trim="formData.newPassword"
          size="large"
          type="password"
          placeholder="请输入新密码"
        >
          <template #prefix>
            <span class="iconfont icon-mima"></span>
          </template>
        </el-input>
      </el-form-item>

      <!--确认密码-->
      <el-form-item label="新密码" prop="rePassword">
        <el-input
          v-model.trim="formData.rePassword"
          size="large"
          type="password"
          placeholder="请确认密码"
        >
          <template #prefix>
            <span class="iconfont icon-mima"></span>
          </template>
        </el-input>
      </el-form-item>
    </el-form>
  </Dialog>
</template>

<script setup>
import { nextTick, ref, getCurrentInstance } from 'vue'
import md5 from 'js-md5'
const { proxy } = getCurrentInstance()

const userInfo = ref(JSON.parse(sessionStorage.getItem('userInfo')) || { menus: [] })
const api = {
  updatePassword: '/Account/UpdatePassword',
}
const dialogConfig = ref({
  show: false,
  title: '修改密码',
  buttons: [
    {
      text: '确定',
      type: 'primary',
      click: () => {
        save()
      },
    },
  ],
})

//校验两次密码是否一致
const checkRePassword = (rule, value, callback) => {
  if (value != formData.value.newPassword) {
    callback(new Error(rule.messgae))
  } else {
    callback()
  }
}
const formData = ref({})
const formDataRef = ref()
const rules = {
  password: [
    { required: true, message: '请输入密码' },
    { validator: proxy.Verify.password, message: '密码只能是数字、字母、特殊字符8-18位' },
  ],
  newPassword: [
    { required: true, message: '请输入新密码' },
    { validator: proxy.Verify.password, message: '密码只能是数字、字母、特殊字符8-18位' },
  ],
  rePassword: [
    { required: true, message: '请确认密码' },
    { validator: checkRePassword, message: '两次输入的密码不一致' },
  ],
}

//修改密码
const save = () => {
  formDataRef.value.validate(async (valid) => {
    if (!valid) {
      return
    }
    let params = {
      userId: userInfo.value.account.userId,
      oldPassword: md5(formData.value.password),
      NewPassword: md5(formData.value.newPassword),
    }
    let result = await proxy.Request({
      url: api.updatePassword,
      method: 'post',
      dataType: 'json',
      params: params,
    })
    if (!result) {
      return
    }

    proxy.Message.success('修改成功')
    dialogConfig.value.show = false
  })
}

//显示
const show = () => {
  dialogConfig.value.show = true
  nextTick(() => {
    formDataRef.value.resetFields()
    formData.value = {}
  })
}

defineExpose({
  show,
})
</script>

<style lang="scss" scoped></style>

<template>
  <Dialog
    :title="dialogConfig.title"
    :show="dialogConfig.show"
    :showClose="dialogConfig.showClose"
    :showCancel="dialogConfig.showCancel"
    :top="dialogConfig.top"
    :buttons="dialogConfig.buttons"
    @close="dialogConfig.show = false"
  >
    <el-form :model="formData" ref="formDataRef" :rules="rules" label-width="80px" @submit.prevent>
      <el-form-item label="原始密码" prop="oldPassword">
        <el-input
          v-model="formData.oldPassword"
          type="password"
          placeholder="请输入密码"
          show-password
        ></el-input>
      </el-form-item>
      <el-form-item label="新密码" prop="newPassword">
        <el-input
          v-model="formData.newPassword"
          type="password"
          placeholder="请输入密码"
          show-password
        ></el-input>
      </el-form-item>
      <el-form-item label="重复密码" prop="rePassword">
        <el-input
          v-model="formData.rePassword"
          type="password"
          placeholder="请再次输入密码"
          show-password
        ></el-input>
      </el-form-item>
    </el-form>
  </Dialog>
</template>

<script setup>
import { nextTick, ref, getCurrentInstance } from 'vue'
import md5 from 'js-md5'
const { proxy } = getCurrentInstance()

const api = {
  UpdatePassword: '/Account/UpdatePassword',
}

const dialogConfig = ref({
  title: '修改密码',
  show: false,
  showClose: true,
  showCancel: true,
  top: 50,
  buttons: [
    {
      type: 'primary',
      text: '保存',
      click: () => {
        submit()
      },
    },
  ],
})

const formDataRef = ref(null)
const formData = ref({})
//校验两次密码是否一致
const checkRePassword = (rule, value, callback) => {
  if (value != formData.value.newPassword) {
    callback(new Error(rule.messgae))
  } else {
    callback()
  }
}
const rules = {
  oldPassword: [
    { required: true, message: '请输入密码', trigger: 'blur' },
    {
      validator: proxy.Verify.password,
      message: '密码最少8位,只能是数字和特殊字符',
    },
  ],
  newPassword: [
    { required: true, message: '请输入密码', trigger: 'blur' },
    {
      validator: proxy.Verify.password,
      message: '密码最少8位,只能是数字和特殊字符',
    },
  ],
  rePassword: [
    { required: true, message: '请再次输入密码', trigger: 'blur' },
    { validator: checkRePassword, message: '两次输入的密码不一致' },
  ],
}

const open = (data) => {
  dialogConfig.value.show = true
  nextTick(() => {
    formDataRef.value.resetFields()
    debugger
    formData.value = { userId: data.userId }
  })
}

//保存
const submit = () => {
  formDataRef.value.validate(async (valid) => {
    if (!valid) return
    let params = {}
    Object.assign(params, formData.value)
    params.oldPassword = md5(params.oldPassword)
    params.newPassword = md5(params.newPassword)

    let result = await proxy.Request({
      url: api.UpdatePassword,
      dataType: 'json',
      method: 'post',
      params: params,
    })
    if (!result) return
    dialogConfig.value.show = false
    proxy.Message.success('修改成功')
  })
}

defineExpose({
  open,
})
</script>

<style lang="scss" scoped>
.check-span-item {
  float: left;
  margin-right: 10px;
  line-height: 20px;
}
</style>

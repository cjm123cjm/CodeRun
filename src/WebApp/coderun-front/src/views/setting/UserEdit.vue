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
      <el-form-item label="用户名" prop="userName">
        <el-input v-model="formData.userName" placeholder="请输入用户名" :maxLength="20"></el-input>
      </el-form-item>
      <el-form-item label="手机号" prop="phone">
        <el-input v-model="formData.phone" placeholder="请输入手机号" :maxLength="11"></el-input>
      </el-form-item>
      <el-form-item label="密码" prop="password" v-if="formData.userId == 0">
        <el-input
          v-model="formData.password"
          type="password"
          placeholder="请输入密码"
          :maxLength="11"
        ></el-input>
      </el-form-item>
      <el-form-item label="重复密码" prop="rePassword" v-if="formData.userId == 0">
        <el-input
          v-model="formData.rePassword"
          type="password"
          placeholder="请再次输入密码"
          :maxLength="11"
        ></el-input>
      </el-form-item>
      <el-form-item label="职位" prop="position">
        <el-input v-model="formData.position" placeholder="请输入职位"></el-input>
      </el-form-item>
      <el-form-item label="角色" prop="roles">
        <el-checkbox-group v-model="formData.roles">
          <span class="check-span-item" v-for="(item, index) in roleList" :key="index">
            <el-checkbox :label="item.roleId + ''">{{ item.roleName }}</el-checkbox>
          </span>
        </el-checkbox-group>
      </el-form-item>
    </el-form>
  </Dialog>
</template>

<script setup>
import { nextTick, ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  UpdateAccount: '/Account/UpdateAccount',
  AddAccount: '/Account/AddAccount',
  LoadRoleList: '/Role/LoadRoleList',
}

//获取用户角色
const roleList = ref([])
const getRoleList = async () => {
  let result = await proxy.Request({
    url: api.LoadRoleList,
    method: 'get',
    dataType: 'json',
  })
  if (!result) return
  roleList.value = result.result
}
getRoleList()

const dialogConfig = ref({
  title: '编辑用户',
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
  if (value != formData.value.password) {
    callback(new Error(rule.messgae))
  } else {
    callback()
  }
}
const rules = {
  userName: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  phone: [
    { required: true, message: '请输入手机号', trigger: 'blur' },
    { validator: proxy.Verify.phone, message: '请输入正确的手机号', trigger: 'blur' },
  ],
  password: [
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
    if (!data.userId) {
      dialogConfig.value.title = '新增用户'
      formData.value = { roles: [], userId: 0 }
    } else {
      dialogConfig.value.title = '修改用户'
      let editData = Object.assign({}, data)
      editData.roles = editData.roles.split(',')
      formData.value = Object.assign({}, editData)
    }
  })
}
const emits = defineEmits(['reload'])

//保存
const submit = () => {
  formDataRef.value.validate(async (valid) => {
    if (!valid) return
    let params = {}
    Object.assign(params, formData.value)
    params.roles = params.roles.join(',')
    delete params.createTime

    let result = await proxy.Request({
      url: params.userId == 0 ? api.AddAccount : api.UpdateAccount,
      dataType: 'json',
      method: 'post',
      params: params,
    })
    if (!result) return
    dialogConfig.value.show = false
    proxy.Message.success('保存成功')
    emits('reload')
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

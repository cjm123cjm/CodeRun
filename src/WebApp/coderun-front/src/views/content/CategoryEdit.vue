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
      <el-form-item label="分类名称" prop="categoryName">
        <el-input
          v-model="formData.categoryName"
          placeholder="请输入分类名称"
          :maxLength="20"
        ></el-input>
      </el-form-item>

      <el-form-item label="封面类型" prop="coverType">
        <el-radio-group v-model="formData.coverType">
          <el-radio :label="0">背景颜色</el-radio>
          <el-radio :label="1">图片</el-radio>
        </el-radio-group>
      </el-form-item>

      <el-form-item label="背景颜色" prop="bgColor" v-if="formData.coverType == 0">
        <el-color-picker v-model="formData.bgColor"></el-color-picker>
      </el-form-item>

      <el-form-item label="图片封面" prop="iconPath" v-if="formData.coverType == 1">
        <CoverUpload v-model="formData.iconPath" :type="0"></CoverUpload>
      </el-form-item>

      <el-form-item label="类型" prop="type">
        <el-radio-group v-model="formData.type">
          <el-radio :label="0">问题分类</el-radio>
          <el-radio :label="1">考题分类</el-radio>
          <el-radio :label="2">问题/考题分类</el-radio>
        </el-radio-group>
      </el-form-item>
    </el-form>
  </Dialog>
</template>

<script setup>
import { nextTick, ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  SaveCategory: '/Category/SaveCategory',
}

const dialogConfig = ref({
  title: '编辑分类',
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
const rules = {
  categoryName: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  coverType: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  type: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  bgColor: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
  iconPath: [{ required: true, message: '请输入用户名', trigger: 'blur' }],
}

const open = (data) => {
  dialogConfig.value.show = true
  nextTick(() => {
    formDataRef.value.resetFields()
    if (!data.categoryId) {
      dialogConfig.value.title = '新增分类'
      formData.value = { categoryId: 0 }
    } else {
      dialogConfig.value.title = '修改分类'
      if (data.bgColor != null && data.bgColor != '') {
        data.coverType = 0
      } else if (data.iconPath != null && data.iconPath != '') {
        data.coverType = 1
      }
      formData.value = Object.assign({}, data)
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
    if (params.coverType == 0) {
      params.iconPath = ''
    } else {
      params.bgColor = ''
    }

    let result = await proxy.Request({
      url: api.SaveCategory,
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

<style lang="scss" scoped></style>

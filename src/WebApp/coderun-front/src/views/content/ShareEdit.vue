<template>
  <Dialog
    :title="dialogConfig.title"
    :show="dialogConfig.show"
    :showClose="dialogConfig.showClose"
    :showCancel="dialogConfig.showCancel"
    :top="dialogConfig.top"
    :buttons="dialogConfig.buttons"
    @close="dialogConfig.show = false"
    width="70%"
  >
    <el-form :model="formData" ref="formDataRef" :rules="rules" label-width="80px" @submit.prevent>
      <el-form-item label="标题" prop="title">
        <el-input v-model="formData.title" placeholder="请输入标题" :maxLength="100"></el-input>
      </el-form-item>

      <el-form-item label="封面类型" prop="coverType">
        <el-radio-group v-model="formData.coverType">
          <el-radio :value="0">无封面</el-radio>
          <el-radio :value="1">横幅</el-radio>
          <el-radio :value="2">图标</el-radio>
        </el-radio-group>
      </el-form-item>

      <el-form-item label="封面" prop="coverPath" v-if="formData.coverType != 0">
        <CoverUpload
          v-if="formData.coverType == 1"
          v-model="formData.coverPath"
          :width="330"
          :height="180"
          :type="2"
        ></CoverUpload>
        <CoverUpload
          v-if="formData.coverType == 2"
          v-model="formData.coverPath"
          :width="100"
          :height="100"
          :type="2"
        ></CoverUpload>
      </el-form-item>

      <el-form-item label="分享内容" prop="content">
        <SunEditor v-model="formData.content" :height="400"></SunEditor>
      </el-form-item>
    </el-form>
  </Dialog>
</template>

<script setup>
import { nextTick, ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  SaveShareInfo: '/ShareInfo/SaveShareInfo',
}

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

const formDataRef = ref()
const formData = ref({})
const rules = {
  title: [{ required: true, message: '请输入标题', trigger: 'blur' }],
  coverType: [{ required: true, message: '请选择封面类型', trigger: 'blur' }],
  content: [{ required: true, message: '请输入内容', trigger: 'blur' }],
  coverPath: [{ required: true, message: '请选择封面', trigger: 'blur' }],
}

const open = (data) => {
  dialogConfig.value.show = true
  nextTick(() => {
    formDataRef.value.resetFields()
    if (!data.shareId) {
      dialogConfig.value.title = '新增分享'
      formData.value = { shareId: 0 }
    } else {
      dialogConfig.value.title = '修改分享'
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

    let result = await proxy.Request({
      url: api.SaveShareInfo,
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

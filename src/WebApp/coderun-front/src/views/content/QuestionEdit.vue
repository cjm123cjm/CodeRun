<template>
  <Dialog
    :title="dialogConfig.title"
    :show="dialogConfig.show"
    :showClose="dialogConfig.showClose"
    :showCancel="dialogConfig.showCancel"
    :top="dialogConfig.top"
    :buttons="dialogConfig.buttons"
    @close="dialogConfig.show = false"
    width="90%"
  >
    <el-form :model="formData" ref="formDataRef" :rules="rules" label-width="80px" @submit.prevent>
      <el-row :gutter="10">
        <el-col :span="12">
          <el-form-item label="标题" prop="title">
            <el-input v-model="formData.title" placeholder="请输入标题" :maxLength="100"></el-input>
          </el-form-item>
        </el-col>
        <el-col :span="6">
          <el-form-item label="分类" prop="categoryId">
            <CategorySelector v-model="formData.categoryId"></CategorySelector>
          </el-form-item>
        </el-col>
        <el-col :span="6">
          <el-form-item label="难易程度" prop="difficultyLevel">
            <el-rate v-model="formData.difficultyLevel"></el-rate>
          </el-form-item>
        </el-col>
      </el-row>

      <el-row :gutter="10">
        <el-col :span="12">
          <el-form-item label="问题" prop="question">
            <SunEditor v-model="formData.question"></SunEditor>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="答案解析" prop="answerAnalysis">
            <SunEditor v-model="formData.answerAnalysis"></SunEditor>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
  </Dialog>
</template>

<script setup>
import CategorySelector from '@/components/content/CategorySelector.vue'
import { nextTick, ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  QuestionInfoAddOrUpdate: '/QuestionInfo/QuestionInfoAddOrUpdate',
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
  categoryId: [{ required: true, message: '请选择分类', trigger: 'blur' }],
  difficultyLevel: [{ required: true, message: '请选择难易程度', trigger: 'blur' }],
  question: [{ required: true, message: '请输入问题', trigger: 'blur' }],
  answerAnalysis: [{ required: true, message: '请输入答案', trigger: 'blur' }],
}

const open = (data) => {
  dialogConfig.value.show = true
  nextTick(() => {
    formDataRef.value.resetFields()
    if (!data.questionId) {
      dialogConfig.value.title = '新增八股文'
      formData.value = { questionId: 0 }
    } else {
      dialogConfig.value.title = '修改八股文'
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
      url: api.QuestionInfoAddOrUpdate,
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

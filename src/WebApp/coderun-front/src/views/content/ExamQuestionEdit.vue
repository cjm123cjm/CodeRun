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

          <el-form-item label="分类" prop="categoryId">
            <CategorySelector v-model="formData.categoryId"></CategorySelector>
          </el-form-item>

          <el-form-item label="难易程度" prop="difficultyLevel">
            <el-rate v-model="formData.difficultyLevel"></el-rate>
          </el-form-item>

          <el-form-item label="问题类型" prop="questionType">
            <el-radio-group v-model="formData.questionType" @change="resetQuestionAnswer">
              <el-radio
                v-for="item in question_type_list"
                :key="item.value"
                :value="item.value"
                :disabled="formData.questionId != 0"
                >{{ item.label }}</el-radio
              >
            </el-radio-group>
          </el-form-item>

          <el-form-item label="答案选项" prop="items" v-if="formData.questionType != 0">
            <div class="question-item" v-for="(item, index) in formData.items">
              <div class="letter">{{ letter[index] }}</div>
              <div class="title">
                <el-form-item
                  label-width="0px"
                  :key="index"
                  :prop="'items.' + index + '.title'"
                  :rules="{
                    required: true,
                    message: '答案选项不能为空',
                  }"
                >
                  <el-input clearable placeholder="请输入答案" v-model="item.title"></el-input>
                </el-form-item>
              </div>
              <div class="op">
                <span class="iconfont icon-jia" v-if="index == 0" @click="addQuestionItem"></span>
                <span class="iconfont icon-jian" v-else @click="removeQuestionItem(index)"></span>
              </div>
            </div>
          </el-form-item>

          <el-form-item label="答案" prop="questionAnswer">
            <!--判断题-->
            <template v-if="currentQuestionType == 0">
              <el-radio-group v-model="formData.questionAnswer">
                <el-radio value="1">正确</el-radio>
                <el-radio value="0">错误</el-radio>
              </el-radio-group>
            </template>

            <!--单选题-->
            <template v-if="currentQuestionType == 1">
              <el-radio-group v-model="formData.questionAnswer">
                <el-radio :value="index + ''" v-for="(value, index) in formData.items">{{
                  letter[index]
                }}</el-radio>
              </el-radio-group>
            </template>

            <!--多选题-->
            <template v-if="currentQuestionType == 2">
              <el-checkbox-group v-model="formData.questionAnswer">
                <el-checkbox :value="index + ''" v-for="(value, index) in formData.items">{{
                  letter[index]
                }}</el-checkbox>
              </el-checkbox-group>
            </template>
          </el-form-item>
        </el-col>
        <el-col :span="12">
          <el-form-item label="问题描述" prop="title">
            <SunEditor v-model="formData.question" :height="200"></SunEditor>
          </el-form-item>
          <el-form-item label="答案分析" prop="title">
            <SunEditor v-model="formData.answerAnalysis" :height="200"></SunEditor>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
  </Dialog>
</template>

<script setup>
import CategorySelector from '@/components/content/CategorySelector.vue'
import { nextTick, ref, getCurrentInstance, computed } from 'vue'
import { question_type, letter } from '@/utils/Constants'
const { proxy } = getCurrentInstance()

const api = {
  SaveExamQuestion: '/ExamQuestion/SaveExamQuestion',
}

const question_type_list = computed(() => {
  const questionList = []
  for (const key in question_type) {
    questionList.push({ label: question_type[key], value: Number.parseInt(key) })
  }
  return questionList
})

const dialogConfig = ref({
  title: '编辑',
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
  answerAnalysis: [{ required: true, message: '请输入答案分析', trigger: 'blur' }],
  questionType: [{ required: true, message: '请输入问题类型', trigger: 'blur' }],
  questionAnswer: [{ required: true, message: '请输入答案', trigger: 'blur' }],
}

//添加选项
const addQuestionItem = () => {
  if (formData.value.items >= 26) {
    proxy.Message.warning('答案选项不能超过26个')
    return
  }
  formData.value.items.push({ title: '', sort: formData.value.items.length + 1 })
}
//移除选项
const removeQuestionItem = (index) => {
  formData.value.items.splice(index, 1)
}
//答案选项切换
const currentQuestionType = ref(0)
const resetQuestionAnswer = (e) => {
  if (e == 2) {
    formData.value.questionAnswer = []
  } else {
    formData.value.questionAnswer = undefined
  }
  currentQuestionType.value = e
}

//打开
const open = (data) => {
  dialogConfig.value.show = true
  nextTick(() => {
    formDataRef.value.resetFields()
    if (!data.questionId) {
      dialogConfig.value.title = '新增考题'
      formData.value = { questionId: 0 }
    } else {
      dialogConfig.value.title = '修改考题'
      formData.value = Object.assign({}, data)
    }
    if (formData.value.items == null) {
      formData.value.items = [
        {
          title: '',
          sort: 1,
        },
      ]
    }
    currentQuestionType.value = formData.value.questionType || 0

    if (formData.value.questionType == 2) {
      debugger
      formData.value.questionAnswer = formData.value.questionAnswer.split(',')
    }
  })
}

//判断答案选项是否重复
const isRepeat = (arr) => {
  let tempMap = {}
  for (let i in arr) {
    if (tempMap[arr[i].title]) {
      return true
    }
    tempMap[arr[i].title] = true
  }
}

const emits = defineEmits(['reload'])
//保存
const submit = () => {
  if (formData.value.questionAnswer && formData.value.questionType != 0) {
    let questionAnswerArray =
      formData.value.questionType == 2
        ? formData.value.questionAnswer
        : [formData.value.questionAnswer]
    for (let i = 0; i < questionAnswerArray.length; i++) {
      if (formData.value.questionAnswer.length - 1 < Number.parseInt(questionAnswerArray[i])) {
        if (formData.value.questionType == 2) {
          formData.value.questionAnswer.splice(i, 1)
        }
        if (formData.value.questionType == 1) {
          formData.value.questionAnswer = null
        }
      }
    }
  }

  formDataRef.value.validate(async (valid) => {
    if (!valid) return
    let params = {}
    Object.assign(params, formData.value)
    if (isRepeat(formData.value.items)) {
      proxy.Message.warning('选项重复')
      return
    }

    if (params.questionType == 2) {
      params.questionAnswer = params.questionAnswer.sort().join(',')
    }

    let result = await proxy.Request({
      url: api.SaveExamQuestion,
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
.question-item {
  width: 100%;
  display: flex;
  .letter {
    width: 20px;
  }
  .title {
    flex: 1;
    .el-form-item {
      margin-bottom: 18px;
    }
  }
  .op {
    width: 100px;
    overflow: hidden;
    .iconfont {
      margin: 0px 10px;
      cursor: pointer;
    }
  }
}
</style>

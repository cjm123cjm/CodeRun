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
      <el-form-item label="轮播图" prop="imgPath">
        <CoverUpload v-model="formData.imgPath" :type="2" :width="330" :height="180"></CoverUpload>
      </el-form-item>

      <el-form-item label="类型" prop="objectType">
        <el-radio-group v-model="formData.objectType">
          <el-radio :value="value.value" v-for="value in object_type_list">{{
            value.label
          }}</el-radio>
        </el-radio-group>
      </el-form-item>

      <el-form-item label="文章ID" prop="objectId" v-if="formData.objectType != 3">
        <el-input v-model="formData.objectId" placeholder="请输入文章ID"></el-input>
      </el-form-item>

      <el-form-item label="外部链接" prop="outerLink" v-if="formData.objectType == 3">
        <el-input v-model="formData.outerLink" placeholder="请输入外部链接"></el-input>
      </el-form-item>
    </el-form>
  </Dialog>
</template>

<script setup>
import { ObjectType } from '@/utils/Constants'
import { nextTick, ref, getCurrentInstance, computed } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  SaveAppCarousel: '/AppCarousel/SaveAppCarousel',
}

const object_type_list = computed(() => {
  const questionList = []
  for (const key in ObjectType) {
    questionList.push({ label: ObjectType[key], value: Number.parseInt(key) })
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

const formDataRef = ref(null)
const formData = ref({})
const rules = {
  imgPath: [{ required: true, message: '请上传轮播图', trigger: 'blur' }],
  objectType: [{ required: true, message: '请输入类型', trigger: 'blur' }],
  objectId: [{ required: true, message: '请输入文章id', trigger: 'blur' }],
  outerLink: [{ required: true, message: '请输入外部链接', trigger: 'blur' }],
}

const open = (data) => {
  dialogConfig.value.show = true
  nextTick(() => {
    formDataRef.value.resetFields()
    if (!data.carouselId) {
      dialogConfig.value.title = '新增轮播图'
      formData.value = { carouselId: 0 }
    } else {
      dialogConfig.value.title = '修改轮播图'
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
      url: api.SaveAppCarousel,
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

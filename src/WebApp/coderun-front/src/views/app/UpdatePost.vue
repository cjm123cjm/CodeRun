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
    <el-form :model="formData" ref="formDataRef" :rules="rules" label-width="100px" @submit.prevent>
      <el-form-item label="版本号">
        {{ formData.version }}
      </el-form-item>

      <el-form-item label="发布类型" prop="status">
        <el-radio-group v-model="formData.status">
          <el-radio :value="0">取消发布</el-radio>
          <el-radio :value="1">灰度发布</el-radio>
          <el-radio :value="2">全网发布</el-radio>
        </el-radio-group>
      </el-form-item>

      <el-form-item label="灰度设备ID" prop="grayscaleDevice" v-if="formData.status == 1">
        <div class="tag-panel">
          <el-tag
            v-for="(tag, index) in formData.grayscaleDevice"
            :key="tag"
            closable
            @close="closeTag(index)"
            :type="tag.type"
            class="tag"
            >{{ tag }}</el-tag
          >
          <div class="tag input" v-if="showInput">
            <el-input
              size="small"
              clearable
              placeholder="请输入设备ID"
              v-model="tagInput"
              @blur="addDeviceId"
              @keyup.enter="addDeviceId"
            ></el-input>
          </div>
          <div v-else class="tag">
            <el-button type="primary" size="small" @click="showInputHandler"> 新增 </el-button>
          </div>
        </div>
      </el-form-item>
    </el-form>
  </Dialog>
</template>

<script setup>
import { nextTick, ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  PostUpdate: '/AppUpdate/PostUpdate',
}

const dialogConfig = ref({
  title: '发布',
  show: false,
  showClose: true,
  showCancel: true,
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
  status: [{ required: true, message: '请输入发布类型', trigger: 'blur' }],
}

const showInput = ref(false)
const showInputHandler = () => {
  showInput.value = true
}
const tagInput = ref('')
const addDeviceId = () => {
  if (tagInput.value) {
    formData.value.grayscaleDevice.push(tagInput.value)
  }
  tagInput.value = ''
  showInput.value = false
}
const closeTag = (index) => {
  formData.value.grayscaleDevice.splice(index, 1)
}

const open = (data) => {
  dialogConfig.value.show = true
  nextTick(() => {
    formDataRef.value.resetFields()
    formData.value = Object.assign({}, data)
    if (!formData.value.grayscaleDevice) {
      formData.value.grayscaleDevice = []
    } else {
      formData.value.grayscaleDevice = formData.value.grayscaleDevice.split(',')
    }
  })
}
const emits = defineEmits(['reload'])

//保存
const submit = () => {
  formDataRef.value.validate(async (valid) => {
    if (!valid) return
    let params = {
      appUpdateId: formData.value.id,
      grayscaleDevice: formData.value.grayscaleDevice
        ? formData.value.grayscaleDevice.join(',')
        : formData.value.grayscaleDevice,
      status: formData.value.status,
    }

    let result = await proxy.Request({
      url: api.PostUpdate,
      dataType: 'json',
      method: 'post',
      params: params,
    })
    if (!result) return
    dialogConfig.value.show = false
    proxy.Message.success('发布成功')
    emits('reload')
  })
}

defineExpose({
  open,
})
</script>

<style lang="scss" scoped>
.tag-panel {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  .tag {
    margin: 0px 5px 5px 0px;
  }
  .input {
    width: 150px;
  }
}
</style>

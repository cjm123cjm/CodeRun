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
      <el-form-item label="版本号" prop="version">
        <el-input
          v-model="formData.version"
          placeholder="请输入版本号,eg:1.0.0"
          :maxLength="20"
        ></el-input>
      </el-form-item>

      <el-form-item label="文件" prop="fileName" class="file-select">
        <div class="file-name">{{ formData.fileName }}</div>
        <el-upload
          name="file"
          :show-file-list="false"
          accept=".apk,.wgt"
          :multiple="false"
          :http-request="selectFile"
        >
          <el-button type="primary" size="small">选择文件</el-button>
        </el-upload>
      </el-form-item>

      <el-form-item label="更新类型" prop="updateType">
        <el-radio-group v-model="formData.updateType">
          <el-radio :value="1">局部热更新</el-radio>
          <el-radio :value="0">全更新</el-radio>
        </el-radio-group>
      </el-form-item>

      <el-form-item label="更新内容" class="update-form-item">
        <div class="update-desc-item" v-for="(item, index) in formData.updateDescList">
          <el-form-item
            :prop="'updateDescList.' + index + '.title'"
            :rules="{
              required: true,
              message: '更新描述不能为空',
            }"
          >
            <div class="update-desc">
              <div class="num">{{ index + 1 }}</div>
              <div class="input">
                <el-input clearable placeholder="更新描述" v-model="item.title"></el-input>
              </div>
              <div class="iconfont icon-jia" v-if="index == 0" @click="addLine"></div>
              <div class="iconfont icon-jian" v-if="index > 0" @click="removeLine(index)"></div>
            </div>
          </el-form-item>
        </div>
      </el-form-item>
    </el-form>
  </Dialog>
</template>

<script setup>
import { nextTick, ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  SaveAppUpdate: '/AppUpdate/SaveAppUpdate',
}

const dialogConfig = ref({
  title: '更新内容',
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
  version: [{ required: true, message: '请输入版本号', trigger: 'blur' }],
  fileName: [{ required: true, message: '请选择文件', trigger: 'blur' }],
  updateType: [{ required: true, message: '请选择更新类型', trigger: 'blur' }],
}

//选择文件
const selectFile = (file) => {
  file = file.file
  formData.value.file = file
  formData.value.fileName = file.name
}

//添加更新内容
const addLine = () => {
  formData.value.updateDescList.push({ title: '' })
}
//移除更新内容
const removeLine = (index) => {
  formData.value.updateDescList.splice(index, 1)
}

const open = (data) => {
  dialogConfig.value.show = true
  nextTick(() => {
    formDataRef.value.resetFields()
    let editData = Object.assign({}, data)
    if (editData.id) {
      editData.updateDescList = editData.updateDescList.map((item) => {
        return { title: item }
      })
      editData.fileName = editData.version + (data.updateType == 0 ? '.apk' : '.wgt')
    } else {
      editData.updateDescList = [{ title: '' }]
    }
    formData.value = Object.assign({}, editData)
  })
}
const emits = defineEmits(['reload'])

//保存
const submit = () => {
  formDataRef.value.validate(async (valid) => {
    if (!valid) return
    let params = {}
    Object.assign(params, formData.value)
    const updateDescArray = params.updateDescList.map((t) => t.title)
    params.updateDesc = updateDescArray.join('|')

    let result = await proxy.Request({
      url: api.SaveAppUpdate,
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
.file-select {
  display: flex;
  .file-name {
    color: #409eff;
    margin-right: 10px;
  }
}
.update-form-item {
  margin-bottom: 0px;
  .update-desc-item {
    width: 100%;
    margin-bottom: 15px;
    .update-desc {
      width: 100%;
      display: flex;
      .num {
        width: 15px;
        margin-right: 2px;
      }
      .input {
        flex: 1;
      }
      .iconfont {
        cursor: pointer;
        margin-left: 10px;
        text-align: right;
      }
    }
  }
}
</style>

<template>
  <Dialog
    :show="dialogConfig.show"
    :title="dialogConfig.title"
    :showClose="true"
    :showCancel="false"
    @close="dialogConfig.show = false"
    :buttons="dialogConfig.buttons"
    width="500px"
  >
    <div class="setp">1、下载导入模板</div>
    <div class="content">
      <a
        :href="`/api/QuestionInfo/DownloadTemplate`"
        v-if="text == '八股文'"
        class="a-link"
        target="_blank"
      >
        <span class="iconfont icon-xiazai">下载模板</span>
      </a>
    </div>
    <div class="setp">2、选择导入文件</div>
    <div class="content">
      <el-upload
        name="file"
        :show-file-list="false"
        accept=".xlsx"
        :multiple="false"
        :http-request="importData"
      >
        <el-button type="primary"> <span class="iconfont icon-up"></span>上传 </el-button>
      </el-upload>
      <div class="tips">仅支持xlsx文件</div>
    </div>
  </Dialog>
  <ImportError ref="importErrorRef"></ImportError>
</template>

<script setup>
import ImportError from './ImportError.vue'
import { ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const props = defineProps({
  text: {
    type: String,
  },
})

const api = {
  ImportQuestionInfo: '/QuestionInfo/ImportQuestionInfo',
}
const dialogConfig = ref({
  title: '导入',
  show: false,
  buttons: [
    {
      text: '关闭',
      type: 'primary',
      click: () => {
        dialogConfig.value.show = false
      },
    },
  ],
})

const open = () => {
  dialogConfig.value.show = true
}

const emit = defineEmits(['reload'])

const importErrorRef = ref()
//上传
const importData = async (file) => {
  file = file.file
  if (props.text == '八股文') {
    let result = await proxy.Request({
      url: api.ImportQuestionInfo,
      method: 'post',
      params: {
        formFile: file,
      },
    })
    if (!result) return
    if (result.result && result.result.length > 0) {
      importErrorRef.value.open(result.result)
    } else {
      proxy.Message.success('导入成功')
      dialogConfig.value.show = false
      emit('reload')
    }
  } else {
    let result = await proxy.Request({
      url: api.ImportQuestionInfo,
      method: 'post',
      params: {
        formFile: file,
      },
    })
    if (!result) return
    if (result.result && result.result.length > 0) {
      importErrorRef.value.open(result.result)
    } else {
      proxy.Message.success('导入成功')
      dialogConfig.value.show = false
      emit('reload')
    }
  }
}

defineExpose({
  open,
})
</script>

<style lang="scss" scoped>
.content {
  margin: 15px 0;
  display: flex;
  align-items: center;
  font-size: 14px;
  .tips {
    margin-left: 10px;
    font-size: 14px;
    color: #878787;
  }
}
.iconfont {
  margin-right: 5px;
}
</style>

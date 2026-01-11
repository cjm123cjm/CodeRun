<template>
  <Dialog
    :title="dialogConfig.title"
    :show="dialogConfig.show"
    :showCancel="false"
    @close="dialogConfig.show = false"
    :width="700"
  >
    <div class="feedback-list">
      <div class="feed-item" v-for="value in dataList">
        <div class="feed-content-my" v-if="data.sendType == 1">
          <div class="content-panel">
            <div class="time">{{ value.createTime }}</div>
            <div class="content">{{ value.content }}</div>
          </div>
          <div class="icon">我</div>
        </div>

        <div class="feed-content-other" v-else>
          <div class="icon">{{ value.nickName.substr(0, 1) }}</div>
          <div class="content-panel">
            <div class="nick-name">{{ value.nickName }}</div>
            <div class="time">{{ value.createTime }}</div>
            <div class="content">{{ value.content }}</div>
          </div>
        </div>
      </div>
    </div>

    <div class="send-panel">
      <el-form v-model="formData" :rules="rules" ref="formDataRef" @submit.prevent>
        <el-form-item prop="content">
          <el-input
            :row="4"
            max="500"
            resize="none"
            show-word-limit
            type="textarea"
            v-model="formData.replyContent"
            placeholder="请输入回复内容"
          ></el-input>
        </el-form-item>
        <el-button class="send-btn" type="primary" @click="submit">发送</el-button>
      </el-form>
    </div>
  </Dialog>
</template>

<script setup>
import { ref, getCurrentInstance, nextTick } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  FeedbackDetail: '/AppFeedback/FeedbackDetail',
  ReplayFeedback: '/AppFeedback/ReplayFeedback',
}

const dialogConfig = ref({
  title: '详情',
  show: false,
})

const pFeedbackId = ref(0)
const open = (data) => {
  dialogConfig.value.show = true
  pFeedbackId.value = data
  loadDataList()
}

const dataList = ref([])
const goBottom = ref(false)
const loadDataList = async () => {
  let result = await proxy.Request({
    url: api.FeedbackDetail,
    dataType: 'json',
    method: 'get',
    params: pFeedbackId.value,
  })
  if (!result) return
  dataList.value = result.result
  if (goBottom.value) {
    nextTick(() => {
      var feedItems = document.querySelectorAll('.feed-item')
      feedItems[feedItems.length - 1].scrollIntoView()
    })
  }
}

const formData = ref({})
const formDataRef = ref()
const rules = {
  content: [{ required: true, message: '请输入回复内容', trigger: 'blur' }],
}

const emits = defineEmits(['reload'])
const submit = () => {
  formDataRef.value.validate(async (valid) => {
    if (!valid) return
    let params = {}
    Object.assign(params, formData.value)
    params.feedbackId = pFeedbackId.value
    let result = await proxy.Request({
      url: api.ReplayFeedback,
      dataType: 'json',
      method: 'post',
      params: params,
    })
    if (!result) return
    goBottom.value = true
    loadDataList()
    formDataRef.value.resetFields()
    emits('reload')
  })
}

defineExpose({
  open,
})
</script>

<style scoped lang="scss">
.feedback-list {
  padding: 10px;
  max-height: calc(100vh * 0.6);
  overflow: auto;
  .feed-item {
    margin-bottom: 20px;
    .icon {
      font-size: 13px;
      display: flex;
      border-radius: 10px;
      width: 50px;
      height: 50px;
      background: #9005fa;
      color: #fff;
      align-items: center;
      justify-content: center;
    }
    .content {
      display: inline-block;
      margin-top: 2px;
      padding: 8px;
      color: #474747;
      border-radius: 5px;
      text-align: left;
    }
    .content-panel {
      flex: 1;
      position: relative;
      &::after {
        content: '';
        position: absolute;
        display: block;
        width: 10px;
        height: 10px;
        background-color: #66f470;
        transform: rotate(45deg);
        border-radius: 2px;
        top: 30px;
      }
    }
    .feed-content-my {
      display: flex;
      padding-left: 60px;
      .icon {
        margin-left: 10px;
      }
      .content-panel {
        text-align: right;
        .time {
          font-size: 13px;
          text-align: right;
        }
        .content {
          background-color: #66f470;
        }
        &::after {
          right: -4px;
        }
      }
    }

    .feed-content-other {
      display: flex;
      padding-right: 60px;
      .icon {
        margin-right: 10px;
        font-size: 18px;
      }
      .content-panel {
        flex: 1;
        position: relative;
        .nick-name {
          font-size: 13px;
        }
        .time {
          font-size: 13px;
          text-align: left;
        }
        .content {
          background-color: #dedede;
        }
        &::after {
          left: -4px;
          top: 45px;
          background-color: #dedede;
        }
      }
    }
  }
}

.send-panel {
  .send-data {
    float: right;
  }
}
</style>

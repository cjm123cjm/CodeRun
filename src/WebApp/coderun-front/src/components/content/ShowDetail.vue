<template>
  <Window :show="showWindow" @close="close">
    <div class="show-detail">
      <div class="iconfont icon-shangyiye" @click="nextAndPre(1)"></div>
      <div class="content-info">
        <div class="header">
          {{ title[showType] }}
        </div>
        <div class="content">
          <div class="title" v-if="showType == 1">{{ detailInfo.title }}</div>
          <div class="detail-info">
            <div class="dif">
              难度:<el-rate v-model="detailInfo.difficultyLevel" :disabled="true"></el-rate>
            </div>
            <div>更新：{{ dayjs(detailInfo.createdTime).format('YYYY-MM-DD HH:mm:ss') }}</div>
          </div>
          <div class="part-title">问题描述：</div>
          <div class="html-content" v-html="detailInfo.question || '暂无问题描述'"></div>

          <!--考题题目选项-->
          <template v-if="showType == 2">
            <div class="part-title">题目类型：{{ question_type[detailInfo.questionType] }}</div>
            <template v-if="detailInfo.questionType != 0">
              <div class="part-title">问题选项：</div>
              <div class="question-item-list">
                <div class="question-item" v-for="(value, index) in detailInfo.items">
                  {{ letter[index] }}、{{ value.title }}
                </div>
              </div>
            </template>
            <div class="part-title">
              答案：
              <template
                v-if="detailInfo.questionType == 0"
                style="margin-left: 5px; margin-top: 5px"
              >
                {{ detailInfo.questionAnswer == 1 ? '正确' : '错误' }}
              </template>
              <template v-else>
                <span v-for="value in (detailInfo.questionAnswer?.split(',') || [])">
                  {{ letter[value] }}
                </span>
              </template>
            </div>
          </template>

          <div class="part-title">答案解析：</div>
          <div class="html-content" v-html="detailInfo.answerAnalysis"></div>
        </div>
      </div>
      <div class="iconfont icon-xiayiye" @click="nextAndPre(2)"></div>
    </div>
  </Window>
</template>

<script setup>
import { question_type, letter } from '@/utils/Constants'
import { ref, getCurrentInstance } from 'vue'
import dayjs from 'dayjs'
const { proxy } = getCurrentInstance()

const props = defineProps({
  showType: {
    type: Number, //1-八股文 2-考题 3-分享
  },
})
const title = ref({
  1: '问题详情',
  2: '考题详情',
  3: '分享详情',
})

const api = {
  showType1: '/QuestionInfo/ShowQuestionInfoDetailNext',
  showType2: '/ExamQuestion/ShowExamQuestionDetailNext',
  showType3: '/ShareInfo/ShowShareInfoDetailNext',
}
const showWindow = ref(false)

//详情
const detailInfo = ref({})
const getDetail = async () => {
  let searchParams = Object.assign({}, params.value)
  if (props.showType == 1) {
    searchParams.currentQuestionInfoId = currentId.value
  } else if (props.showType == 2) {
    searchParams.currentQuestionId = currentId.value
  }
  searchParams.nextType = nextType.value

  let result = await proxy.Request({
    url: api['showType' + props.showType],
    method: 'get',
    dataType: 'json',
    params: searchParams,
  })
  if (!result) return
  detailInfo.value = result.result
  if (props.showType == 3) {
    currentId.value = result.result.shareId
  } else {
    currentId.value = result.result.questionId
  }
}

//当前id
const currentId = ref()
//查询参数
const params = ref({})
const nextType = ref(0)
//打开
const open = (id, searchParams) => {
  showWindow.value = true
  currentId.value = id
  params.value = searchParams
  nextType.value = 0
  getDetail()
  window.addEventListener('keyup', keyHandler, false)
}
//关闭
const close = () => {
  showWindow.value = false
  window.removeEventListener('keyup', keyHandler, false)
}

//按键处理
const keyHandler = (event) => {
  const e = event || window.event || arguments.callee.caller.arguments[0]
  let key = e.keyCode
  if (key == 39) {
    nextAndPre(2) //下一页
  } else if (key == 37) {
    nextAndPre(1) //上一页
  }
}
const nextAndPre = (type) => {
  nextType.value = type
  getDetail()
}

defineExpose({
  open,
})
</script>

<style lang="scss" scoped>
.show-detail {
  height: calc(100vh);
  display: flex;
  justify-content: center;
  align-items: center;
  .content-info {
    width: 430px;
    height: 881px;
    background: url(@/assets/iphone.png);
    background-color: #fff;
    background-position-x: 0px;
    border-radius: 79px;
    padding: 52px 16px 35px 19px;
    background-size: cover;
    .header {
      background: #dad9d9;
      font-size: 18px;
      line-height: 30px;
      text-align: center;
      border-radius: 5px 5px 0px 0px;
      margin-top: 12px;
    }
    .content {
      height: 730px;
      overflow: auto;
      padding: 10px;
      .title {
        font-weight: bold;
        font-size: 15px;
      }
      .detail-info {
        display: flex;
        align-items: center;
        font-size: 13px;
        color: #878787;
        justify-content: space-between;
        .dif {
          display: flex;
          align-items: center;
        }
      }
      .part-title {
        margin-top: 10px;
        font-weight: bold;
        border-left: 3px solid #02baf8;
        padding-left: 5px;
      }
      .html-content {
        margin-top: 10px;

        :deep code {
          word-break: break-all;
          word-wrap: break-word;
          white-space: pre-wrap;
        }

        :deep img {
          max-width: 100%;
        }
      }

      .question-item-list {
        margin-top: 10px;
        padding-left: 10px;
        .question-item {
          margin-top: 5px;
        }
      }
    }
  }

  .iconfont {
    font-size: 50px;
    color: #97def6;
    cursor: pointer;
  }
  .icon-shangyiye {
    margin-right: 20px;
  }
  .icon-xiayiye {
    margin-left: 20px;
  }
}
</style>

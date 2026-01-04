<template>
  <div class="top-panel">
    <el-card>
      <el-form :model="searchForm" label-width="70px" label-position="right">
        <el-row>
          <el-col :span="5">
            <el-form-item label="标题" label-width="50px">
              <el-input
                class="password-input"
                v-model="searchForm.title"
                clearable
                placeholder="请输入标题"
                @keyup.enter="loadDataList"
              ></el-input>
            </el-form-item>
          </el-col>

          <el-col :span="4">
            <el-form-item label="分类" label-width="65px">
              <CategorySelector
                v-model="searchForm.categoryId"
                @change="loadDataList"
              ></CategorySelector>
            </el-form-item>
          </el-col>

          <el-col :span="4">
            <el-form-item label="难度" label-width="60px">
              <el-select v-model="searchForm.difficultyLevel" placeholder="请选择难度" clearable>
                <el-option label="一星" :value="1"></el-option>
                <el-option label="二星" :value="2"></el-option>
                <el-option label="三星" :value="3"></el-option>
                <el-option label="四星" :value="4"></el-option>
                <el-option label="五星" :value="5"></el-option>
              </el-select>
            </el-form-item>
          </el-col>

          <el-col :span="4">
            <el-form-item label="问题类型" label-width="80px">
              <el-select v-model="searchForm.questionType" placeholder="请选择问题类型" clearable>
                <el-option
                  :label="value.label"
                  :value="value.value"
                  v-for="value in question_type_list"
                ></el-option>
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row class="towRow">
          <el-col :span="5">
            <el-form-item label="状态" label-width="50px">
              <el-select v-model="searchForm.status" placeholder="请选择状态" clearable>
                <el-option label="待发布" :value="0"></el-option>
                <el-option label="已发布" :value="1"></el-option>
              </el-select>
            </el-form-item>
          </el-col>

          <el-col :span="4">
            <el-form-item label="创建人" label-width="65px">
              <el-input
                class="password-input"
                v-model="searchForm.createdUserName"
                clearable
                placeholder="支持模糊搜索"
                @keyup.enter="loadDataList"
              ></el-input>
            </el-form-item>
          </el-col>

          <el-col :span="7" :style="{ paddingLeft: '10px' }">
            <el-button-group>
              <el-button type="success" @click="loadDataList">查询</el-button>
              <el-button
                type="primary"
                @click="addQuestion"
                v-has="proxy.PermissionCodes.question.edit"
                >新增</el-button
              >
              <el-button
                type="primary"
                @click="importBatch"
                v-has="proxy.PermissionCodes.exam.import"
                >批量导入</el-button
              >
              <el-button
                type="primary"
                @click="batchPostQuestion"
                v-has="proxy.PermissionCodes.exam.post"
                :disabled="selectRowData.length == 0"
                >批量发布</el-button
              >
              <el-button
                type="danger"
                @click="batchDelQuestion"
                v-has="proxy.PermissionCodes.exam.batchDel"
                :disabled="selectRowData.length == 0"
                >批量删除</el-button
              >
            </el-button-group>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
  </div>
  <el-card class="table-data-card">
    <template #header>
      <span>考题列表</span>
    </template>
    <Table
      ref="tableDataRef"
      :columns="columns"
      :fetch="loadDataList"
      :dataSource="tableData"
      :options="tableOptions"
      :extHeight="tableOptions.extHeight"
      :selected="selectedHandler"
      @rowSelected="rowSelected"
    >
      <template #titleSlot="{ index, row }">
        <a class="a-link" @click="showDetailHandler(row)">{{ row.title }}</a>
      </template>
      <template #difficultyLevelSlot="{ index, row }">
        <el-rate v-model="row.difficultyLevel" :disabled="true"></el-rate>
      </template>
      <template #questionTypeSlot="{ index, row }">
        {{ question_type[row.questionType] }}
      </template>
      <template #statusSlot="{ index, row }">
        <Badge v-if="row.status == 0" text="待发布" showType="orange"></Badge>
        <Badge v-if="row.status == 1" text="已发布" showType="green"></Badge>
      </template>
      <template #createdTimeSlot="{ index, row }">
        {{ dayjs(row.createdTime).format('YYYY-MM-DD HH:mm:ss') }}
      </template>
      <template #operation="{ index, row }">
        <div
          class="row-op-panel"
          v-if="!(userInfo.account.isAdmin && row.userId == userInfo.account.userId)"
        >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="editQuestion(row)"
            v-has="proxy.PermissionCodes.exam.edit"
            v-if="row.status == 0"
            >修改</a
          >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="delQuestion(row)"
            v-has="proxy.PermissionCodes.exam.del"
            v-if="
              row.status == 0 &&
              (row.createdUserId == userInfo.account.userId || userInfo.account.isAdmin)
            "
            >删除</a
          >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="postQuestion(row)"
            v-has="proxy.PermissionCodes.exam.del"
            v-if="row.status == 0"
            >发布</a
          >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="cancelQuestion(row)"
            v-has="proxy.PermissionCodes.exam.del"
            v-if="row.status == 1"
            >取消发布</a
          >
        </div>
      </template>
    </Table>
  </el-card>
  <ExamQuestionEdit ref="examQuestionEditRef" @reload="loadDataList"></ExamQuestionEdit>
  <ImportData ref="importDataRef" text="考题" @reload="loadDataList"></ImportData>
  <ShowDetail ref="showDetailRef" :showType="2"></ShowDetail>
</template>

<script setup>
import CategorySelector from '@/components/content/CategorySelector.vue'
import ExamQuestionEdit from './ExamQuestionEdit.vue'
import Badge from '@/components/Badge.vue'
import ShowDetail from '@/components/content/ShowDetail.vue'
import dayjs from 'dayjs'
import { question_type } from '@/utils/Constants'
import { ref, getCurrentInstance, computed } from 'vue'
const { proxy } = getCurrentInstance()

const userInfo = ref(JSON.parse(sessionStorage.getItem('userInfo')))

const api = {
  LoadExamQuestionList: '/ExamQuestion/LoadExamQuestionList',
  DeletedExamQuestion: '/ExamQuestion/DeletedExamQuestion',
  DeletedBatchExamQuestion: '/ExamQuestion/DeletedBatchExamQuestion',
  PostExamQuestion: '/ExamQuestion/PostExamQuestion',
  CancelExamQuestion: '/ExamQuestion/CancelExamQuestion',
  ExamQuestionById: '/ExamQuestion/ExamQuestionById/',
}

const question_type_list = computed(() => {
  const questionList = []
  for (const key in question_type) {
    questionList.push({ label: question_type[key], value: key })
  }
  return questionList
})

const searchForm = ref({})
const tableData = ref({
  pageIndex: 1,
  pageSize: 30,
})
const tableDataRef = ref()
const tableOptions = ref({
  selectType: 'checkbox',
  extHeight: 155,
  showIndex: true,
})

//加载列表
const loadDataList = async () => {
  let params = {
    pageIndex: tableData.value.pageIndex,
    pageSize: tableData.value.pageSize,
  }
  Object.assign(params, searchForm.value)
  const result = await proxy.Request({
    url: api.LoadExamQuestionList,
    method: 'get',
    dataType: 'json',
    params: params,
  })
  if (!result) return

  tableData.value = result.result
}

const columns = [
  {
    label: '标题',
    prop: 'title',
    scopedSlots: 'titleSlot',
  },
  {
    label: '分类',
    prop: 'categoryName',
    width: 120,
  },
  {
    label: '难度',
    prop: 'difficultyLevel',
    scopedSlots: 'difficultyLevelSlot',
    width: 150,
  },
  {
    label: '问题类型',
    prop: 'questionType',
    scopedSlots: 'questionTypeSlot',
    width: 150,
  },
  {
    label: '状态',
    prop: 'status',
    scopedSlots: 'statusSlot',
    width: 120,
  },
  {
    label: '创建人',
    prop: 'createdUserName',
    width: 200,
  },
  {
    label: '创建时间',
    prop: 'createdTime',
    scopedSlots: 'createdTimeSlot',
    width: 200,
  },
  {
    label: '操作',
    width: 150,
    scopedSlots: 'operation',
  },
]

const examQuestionEditRef = ref()
//新增
const addQuestion = () => {
  examQuestionEditRef.value.open({ questionId: 0 })
}
//修改
const editQuestion = async (data) => {
  let result = await proxy.Request({
    url: api.ExamQuestionById + data.questionId,
    dataType: 'json',
    method: 'get',
  })
  if (!result) return
  examQuestionEditRef.value.open(Object.assign({}, result.result))
}

const importDataRef = ref()
//批量导入
const importBatch = () => {
  importDataRef.value.open()
}
//选择项变化
const selectedHandler = (row, index) => {
  return row.status == 0
}
//已经选中的数据
const selectRowData = ref([])
const rowSelected = (selectedRows) => {
  selectRowData.value = []
  selectedRows.forEach((element) => {
    selectRowData.value.push(element.questionId)
  })
}
//删除
const delQuestion = (row) => {
  proxy.Confirm('确定删除这条数据吗', async () => {
    let result = await proxy.Request({
      url: api.DeletedExamQuestion,
      dataType: 'json',
      method: 'post',
      params: row.questionId,
    })
    if (!result) return
    proxy.Message.success('删除成功')
    loadDataList()
  })
}
//批量删除
const batchDelQuestion = () => {
  if (selectRowData.value.length == 0) {
    proxy.Message.warn('请选择删除的数据')
    return
  }
  proxy.Confirm('确定删除选中的数据吗?', async () => {
    let ids = selectRowData.value.join(',')
    let result = await proxy.Request({
      url: api.DeletedBatchExamQuestion,
      dataType: 'json',
      method: 'post',
      params: ids,
    })
    if (!result) return
    proxy.Message.success('删除成功')
    loadDataList()
  })
}
//批量发布
const batchPostQuestion = async () => {
  if (selectRowData.value.length == 0) {
    proxy.Message.warn('请选择发布的数据')
    return
  }
  let ids = selectRowData.value.join(',')
  await post(ids)
}
const postQuestion = async (row) => {
  await post(row.questionId)
}
const post = async (ids) => {
  let result = await proxy.Request({
    url: api.PostExamQuestion,
    dataType: 'json',
    method: 'post',
    params: ids,
  })
  if (!result) return
  proxy.Message.success('发布成功')
  loadDataList()
}
//取消发布
const cancelQuestion = async (row) => {
  let result = await proxy.Request({
    url: api.CancelExamQuestion,
    dataType: 'json',
    method: 'post',
    params: row.questionId,
  })
  if (!result) return
  proxy.Message.success('取消发布成功')
  loadDataList()
}

//显示详情
const showDetailRef = ref()
const showDetailHandler = (row) => {
  showDetailRef.value.open(row.questionId, searchForm.value)
}
</script>

<style lang="scss" scoped>
.towRow {
  margin-top: 5px;
}
</style>

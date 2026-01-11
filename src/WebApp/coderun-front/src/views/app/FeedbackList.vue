<template>
  <div class="top-panel">
    <el-card>
      <el-form :model="searchForm" label-width="70px" label-position="right">
        <el-row>
          <el-col :span="5">
            <el-form-item label="反馈日期">
              <el-date-picker
                v-model="searchForm.createdTimeRange"
                type="daterange"
                range-separator="至"
                start-placeholder="开始日期"
                end-placeholder="结束日期"
                value-format="YYYY-MM-DD"
                @chage="loadDataList"
              >
              </el-date-picker>
            </el-form-item>
          </el-col>

          <el-col :span="5">
            <el-form-item label="状态" label-width="60px">
              <el-select v-model="searchForm.status" placeholder="请选择难度" clearable>
                <el-option label="未回复" :value="1"></el-option>
                <el-option label="已回复" :value="2"></el-option>
              </el-select>
            </el-form-item>
          </el-col>

          <el-col :span="5">
            <el-form-item label="创建人">
              <el-input
                v-model="searchForm.userName"
                clearable
                placeholder="请输入设备id"
              ></el-input>
            </el-form-item>
          </el-col>

          <el-col :span="4" :style="{ paddingLeft: '10px' }">
            <el-button type="success" @click="loadDataList">查询</el-button>
          </el-col>
        </el-row>
      </el-form>
    </el-card>
  </div>
  <el-card class="table-data-card">
    <template #header>
      <span>反馈列表</span>
    </template>
    <Table
      ref="tableDataRef"
      :columns="columns"
      :fetch="loadDataList"
      :dataSource="tableData"
      :options="tableOptions"
      :extHeight="tableOptions.extHeight"
    >
      <template #createdTimeSlot="{ index, row }">
        {{ dayjs(row.createdTime).format('YYYY-MM-DD HH:mm:ss') }}
      </template>
      <template #statusSlot="{ index, row }">
        <Badge :showType="green" :text="已回复" v-if="row.status == 1"></Badge>
        <Badge :showType="red" :text="未回复" v-else></Badge>
      </template>
      <template #operation="{ index, row }">
        <div class="row-op-panel">
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="replay(row)"
            v-has="proxy.PermissionCodes.feedback.replay"
            v-if="row.status == 0"
            >回复</a
          >
        </div>
      </template>
    </Table>
  </el-card>
  <FeedbackReplay @reload="loadDataList" ref="feedbackReplayRef"></FeedbackReplay>
</template>

<script setup>
import FeedbackReplay from './FeedbackReplay.vue'
import dayjs from 'dayjs'
import Badge from '@/components/Badge.vue'
import { ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  LoadFeedbackList: '/AppFeedback/LoadFeedbackList',
}

const searchForm = ref({})
const tableData = ref({
  pageIndex: 1,
  pageSize: 30,
})
const tableDataRef = ref()
const tableOptions = ref({
  extHeight: 125,
})

//加载反馈列表
const loadDataList = async () => {
  let params = {
    pageIndex: tableData.value.pageIndex,
    pageSize: tableData.value.pageSize,
  }
  Object.assign(params, searchForm.value)
  if (searchForm.value.createdTimeRange) {
    params.feedbackStartTime = searchForm.value.createdTimeRange[0]
    params.feedbackEndTime = searchForm.value.createdTimeRange[1]
  }
  delete params.createdTimeRange

  const result = await proxy.Request({
    url: api.LoadFeedbackList,
    method: 'get',
    dataType: 'json',
    params: params,
  })
  if (!result) return

  tableData.value = result.result
}

const columns = [
  {
    label: '问题',
    prop: 'content',
  },
  {
    label: '昵称',
    prop: 'nickName',
  },
  {
    label: '回复状态',
    prop: 'status',
    scopedSlots: 'statusSlot',
  },
  {
    label: '创建时间',
    prop: 'createdTime',
    scopedSlots: 'createdTimeSlot',
  },
  {
    label: '操作',
    width: 100,
    scopedSlots: 'operation',
  },
]

const feedbackReplayRef = ref()
const replay = (data) => {
  feedbackReplayRef.value.open(data.feedbackId)
}
</script>

<style lang="scss" scoped></style>

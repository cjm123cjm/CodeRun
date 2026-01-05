<template>
  <div class="top-panel">
    <el-card>
      <el-form :model="searchForm" label-width="70px" label-position="right">
        <el-row>
          <el-col :span="5">
            <el-form-item label="加入日期">
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
            <el-form-item label="最近使用日期" label-width="100px">
              <el-date-picker
                v-model="searchForm.useTimeRange"
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
            <el-form-item label="品牌" label-width="60px">
              <el-input
                v-model="searchForm.deviceBrand"
                clearable
                placeholder="请输入品牌"
              ></el-input>
            </el-form-item>
          </el-col>

          <el-col :span="5">
            <el-form-item label="设备id">
              <el-input
                v-model="searchForm.deviceId"
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
      <span>设备列表</span>
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
      <template #lastUseTimeSlot="{ index, row }">
        {{ dayjs(row.lastUseTime).format('YYYY-MM-DD HH:mm:ss') }}
      </template>
    </Table>
  </el-card>
</template>

<script setup>
import dayjs from 'dayjs'
import { ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  LoadAppDeviceList: '/AppDevice/LoadAppDeviceList',
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

//加载用户列表
const loadDataList = async () => {
  let params = {
    pageIndex: tableData.value.pageIndex,
    pageSize: tableData.value.pageSize,
  }
  Object.assign(params, searchForm.value)
  if (searchForm.value.createdTimeRange) {
    params.joinTimeStart = searchForm.value.createdTimeRange[0]
    params.joinTimeEnd = searchForm.value.createdTimeRange[1]
  }
  if (searchForm.value.useTimeRange) {
    params.lastUseTimeStart = searchForm.value.useTimeRange[0]
    params.lastUseTimeEnd = searchForm.value.useTimeRange[1]
  }
  delete params.createdTimeRange
  delete params.useTimeRange

  const result = await proxy.Request({
    url: api.LoadAppDeviceList,
    method: 'get',
    dataType: 'json',
    params: params,
  })
  if (!result) return

  tableData.value = result.result
}

const columns = [
  {
    label: '设备id',
    prop: 'deviceId',
  },
  {
    label: '手机品牌',
    prop: 'deviceBrand',
  },
  {
    label: '创建时间',
    prop: 'createdTime',
    scopedSlots: 'createdTimeSlot',
  },
  {
    label: '最后使用时间',
    prop: 'lastUseTime',
    scopedSlots: 'lastUseTimeSlot',
  },
  {
    label: 'ip地址',
    prop: 'ip',
  },
]
</script>

<style lang="scss" scoped></style>

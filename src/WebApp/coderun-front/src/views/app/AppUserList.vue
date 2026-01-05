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
            <el-form-item label="邮箱">
              <el-input
                class="password-input"
                v-model="searchForm.email"
                clearable
                placeholder="支持模糊搜索"
                @keyup.enter.native="loadDataList"
              ></el-input>
            </el-form-item>
          </el-col>

          <el-col :span="5">
            <el-form-item label="设备ID">
              <el-input
                class="password-input"
                v-model="searchForm.deviceId"
                clearable
                placeholder="支持模糊搜索"
                @keyup.enter.native="loadDataList"
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
      <span>App用户列表</span>
    </template>
    <Table
      ref="tableDataRef"
      :columns="columns"
      :fetch="loadDataList"
      :dataSource="tableData"
      :options="tableOptions"
      :extHeight="tableOptions.extHeight"
    >
      <template #nickNameSlot="{ index, row }">
        {{ row.nickName }}
        <span v-if="row.sex == 1">(男)</span>
        <span v-if="row.sex == 0">(女)</span>
      </template>
      <template #statusSlot="{ index, row }">
        <span v-if="row.status == 1" style="color: green">启用</span>
        <span v-else style="color: red">禁用</span>
      </template>
      <template #lastLoginTimeSlot="{ index, row }">
        {{ dayjs(row.createdTime).format('YYYY-MM-DD HH:mm:ss') }}
      </template>
      <template #joinTimeSlot="{ index, row }">
        {{ dayjs(row.joinTime).format('YYYY-MM-DD HH:mm:ss') }}
      </template>
      <template #operation="{ index, row }">
        <div class="row-op-panel">
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="changeAccountStatus(row)"
            v-has="proxy.PermissionCodes.appUser.edit"
          >
            {{ row.status == 1 ? '禁用' : '启用' }}
          </a>
        </div>
      </template>
    </Table>
  </el-card>
</template>

<script setup>
import { ref, getCurrentInstance } from 'vue'
import dayjs from 'dayjs'
const { proxy } = getCurrentInstance()

const api = {
  LoadAppUserInfoList: '/AppUserInfo/LoadAppUserInfoList',
  UpdateStatusAppUserInfo: '/AppUserInfo/UpdateStatusAppUserInfo',
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
  delete params.createdTimeRange

  const result = await proxy.Request({
    url: api.LoadAppUserInfoList,
    method: 'get',
    dataType: 'json',
    params: params,
  })
  if (!result) return

  tableData.value = result.result
}

const columns = [
  {
    label: '邮箱',
    prop: 'email',
  },
  {
    label: '昵称',
    prop: 'nickName',
    scopedSlots: 'nickNameSlot',
  },
  {
    label: '加入时间',
    prop: 'joinTime',
    scopedSlots: 'joinTimeSlot',
  },
  {
    label: '最后登录时间',
    prop: 'lastLoginTime',
    scopedSlots: 'lastLoginTimeSlot',
  },
  {
    label: '登陆设备',
    prop: 'lastUseDeviceId',
  },
  {
    label: '设备品牌',
    prop: 'lastUseDeviceBrand',
  },
  {
    label: '最后登录ip',
    prop: 'lastLoginIp',
  },
  {
    label: '状态',
    prop: 'status',
    scopedSlots: 'statusSlot',
  },
  {
    label: '操作',
    width: '200',
    scopedSlots: 'operation',
  },
]

//修改状态
const changeAccountStatus = (data) => {
  let status = data.status == 0 ? 1 : 0
  let info = data.status == 0 ? '启用' : '禁用'
  proxy.Confirm(`确定要【${info}】【${data.userName}】这个账户吗?`, async () => {
    let result = await proxy.Request({
      url: api.UpdateStatusAppUserInfo,
      dataType: 'json',
      method: 'post',
      params: {
        userId: data.userId,
        status: status,
      },
    })
    if (!result) return
    proxy.Message.success('操作成功')
    loadDataList()
  })
}
</script>

<style lang="scss" scoped></style>

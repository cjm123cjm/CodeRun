<template>
  <div class="top-panel">
    <el-card>
      <el-form :model="searchForm" label-width="70px" label-position="right">
        <el-row>
          <el-col :span="5">
            <el-form-item label="用户名">
              <el-input
                class="password-input"
                v-model="searchForm.userName"
                clearable
                placeholder="支持模糊搜索"
                @keyup.enter.native="loadDataList"
              ></el-input>
            </el-form-item>
          </el-col>

          <el-col :span="5">
            <el-form-item label="手机号">
              <el-input
                class="password-input"
                v-model="searchForm.phone"
                clearable
                placeholder="支持模糊搜索"
                @keyup.enter.native="loadDataList"
              ></el-input>
            </el-form-item>
          </el-col>

          <el-col :span="4" :style="{ paddingLeft: '10px' }">
            <el-button type="success" @click="loadDataList">查询</el-button>
            <el-button type="primary" @click="addAccount" v-has="proxy.PermissionCodes.account.edit"
              >新增用户</el-button
            >
          </el-col>
        </el-row>
      </el-form>
    </el-card>
  </div>
  <el-card class="table-data-card">
    <template #header>
      <span>用户列表</span>
    </template>
    <Table
      ref="tableDataRef"
      :columns="columns"
      :fetch="loadDataList"
      :dataSource="tableData"
      :options="tableOptions"
      :extHeight="tableOptions.extHeight"
    >
      <template #statusSlot="{ index, row }">
        <span v-if="row.status == 1" style="color: green">启用</span>
        <span v-else style="color: red">禁用</span>
      </template>
      <template #timeSlot="{ index, row }">
        {{ dayjs(row.createdTime).format('YYYY-MM-DD HH:mm:ss') }}
      </template>
      <template #operation="{ index, row }">
        <div class="row-op-panel">
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="editAccount(row)"
            v-has="proxy.PermissionCodes.account.edit"
            >修改</a
          >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="editPassword(row)"
            v-has="proxy.PermissionCodes.account.updatePwd"
            >修改密码</a
          >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="changeAccountStatus(row)"
            v-has="proxy.PermissionCodes.account.updateStatus"
          >
            {{ row.status == 1 ? '禁用' : '启用' }}
          </a>
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="delAccount(row)"
            v-has="proxy.PermissionCodes.account.del"
            >删除</a
          >
        </div>
      </template>
    </Table>
  </el-card>
  <UserEdit ref="userEditRef" @reload="loadDataList"></UserEdit>
  <UserPasswordEdit ref="userPasswordEditRef"></UserPasswordEdit>
</template>

<script setup>
import UserEdit from './UserEdit.vue'
import UserPasswordEdit from './UserPasswordEdit.vue'
import { ref, getCurrentInstance } from 'vue'
import dayjs from 'dayjs'
const { proxy } = getCurrentInstance()
const api = {
  LoadAccountList: '/Account/LoadAccountList',
  UpdateAccountStatus: '/Account/UpdateAccountStatus',
  DeleteAccount: '/Account/DeleteAccount',
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
  const result = await proxy.Request({
    url: api.LoadAccountList,
    method: 'get',
    dataType: 'json',
    params: params,
  })
  if (!result) return

  tableData.value = result.result
}

const columns = [
  {
    label: '用户名',
    prop: 'userName',
  },
  {
    label: '手机号',
    prop: 'phone',
  },
  {
    label: '职位',
    prop: 'position',
  },
  {
    label: '角色名',
    prop: 'roleNames',
  },
  {
    label: '状态',
    prop: 'status',
    scopedSlots: 'statusSlot',
  },
  {
    label: '创建时间',
    prop: 'createdTime',
    scopedSlots: 'timeSlot',
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
      url: api.UpdateAccountStatus,
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
//删除
const delAccount = (data) => {
  proxy.Confirm(`确定要删除【${data.userName}】吗?`, async () => {
    let result = await proxy.Request({
      url: api.DeleteAccount,
      dataType: 'json',
      method: 'post',
      params: data.userId,
    })
    if (!result) return
    proxy.Message.success('删除成功')
    loadDataList()
  })
}

const userEditRef = ref()
//新增用户
const addAccount = () => {
  userEditRef.value.open({ userId: 0 })
}
//修改用户
const editAccount = (data) => {
  userEditRef.value.open(Object.assign({}, data))
}

const userPasswordEditRef = ref()
//修改密码
const editPassword = (data) => {
  userPasswordEditRef.value.open(data)
}
</script>

<style lang="scss" scoped></style>

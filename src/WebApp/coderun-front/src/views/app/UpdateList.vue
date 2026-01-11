<template>
  <div class="top-panel">
    <el-card>
      <el-form :model="searchForm" label-width="70px" label-position="right">
        <el-row>
          <el-col :span="6">
            <el-form-item label="发布日期">
              <el-date-picker
                v-model="searchForm.postTimeRange"
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

          <el-col :span="4" :style="{ paddingLeft: '10px' }">
            <el-button type="success" @click="loadDataList">查询</el-button>
            <el-button type="primary" v-has="proxy.PermissionCodes.app.edit" @click="addUpdate"
              >发布版本</el-button
            >
          </el-col>
        </el-row>
      </el-form>
    </el-card>
  </div>
  <el-card class="table-data-card">
    <template #header>
      <span>发布列表</span>
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
      <template #updateDescSlot="{ index, row }">
        <div v-for="(item, num) in row.updateDescList">{{ num + 1 }}、{{ item }}</div>
      </template>

      <template #updateTypeSlot="{ index, row }">
        <div v-if="row.updateType == 0">全更新</div>
        <div v-if="row.updateType == 1">局部更新</div>
      </template>

      <template #statusSlot="{ index, row }">
        <Badge showType="red" text="未发布" v-if="row.status == 0"></Badge>
        <Badge showType="yellow" text="灰度发布" v-if="row.status == 1"></Badge>
        <Badge showType="green" text="全网发布" v-if="row.status == 2"></Badge>
      </template>

      <template #operation="{ index, row }">
        <div class="row-op-panel">
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="editUpdate(row)"
            v-has="proxy.PermissionCodes.app.edit"
            >修改</a
          >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="delUpdate(row)"
            v-has="proxy.PermissionCodes.app.edit"
            >删除</a
          >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="postUpdate(row)"
            v-has="proxy.PermissionCodes.app.post"
            >发布</a
          >
        </div>
      </template>
    </Table>
  </el-card>
  <UpdateEdit ref="updateEditRef" @reload="loadDataList"></UpdateEdit>
  <UpdatePost ref="updatePostRef" @reload="loadDataList"></UpdatePost>
</template>

<script setup>
import UpdateEdit from './UpdateEdit.vue'
import UpdatePost from './UpdatePost.vue'
import dayjs from 'dayjs'
import Badge from '@/components/Badge.vue'
import { ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  LoadAppUpdateList: '/AppUpdate/LoadAppUpdateList',
  DeletedAppUpdate: '/AppUpdate/DeletedAppUpdate',
  PostUpdate: '/AppUpdate/PostUpdate',
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

//加载发布列表
const loadDataList = async () => {
  let params = {
    pageIndex: tableData.value.pageIndex,
    pageSize: tableData.value.pageSize,
  }
  Object.assign(params, searchForm.value)
  if (searchForm.value.postTimeRange) {
    params.pulishStartTime = searchForm.value.postTimeRange[0]
    params.pulishEndTime = searchForm.value.postTimeRange[1]
  }
  delete params.postTimeRange

  const result = await proxy.Request({
    url: api.LoadAppUpdateList,
    method: 'get',
    dataType: 'json',
    params: params,
  })
  if (!result) return

  tableData.value = result.result
}

const columns = [
  {
    label: '版本',
    prop: 'version',
  },
  {
    label: '更新内容',
    prop: 'updateDesc',
    scopedSlots: 'updateDescSlot',
  },
  {
    label: '发布时间',
    prop: 'createdTime',
    scopedSlots: 'createdTimeSlot',
  },
  {
    label: '更新类型',
    prop: 'updateType',
    scopedSlots: 'updateTypeSlot',
  },
  {
    label: '状态',
    prop: 'status',
    scopedSlots: 'statusSlot',
  },
  {
    label: '操作',
    width: 150,
    scopedSlots: 'operation',
  },
]

const updateEditRef = ref()
//新增
const addUpdate = () => {
  updateEditRef.value.open({})
}
//修改
const editUpdate = (row) => {
  updateEditRef.value.open(row)
}
//删除
const delUpdate = (row) => {
  proxy.Confirm(`确定删除版本号为【${row.version}】更新数据吗?`, async () => {
    let result = await proxy.Request({
      url: api.DeletedAppUpdate,
      dataType: 'json',
      method: 'post',
      params: row.id,
    })
    if (!result) return
    proxy.Message.success('删除成功')
    loadDataList()
  })
}

const updatePostRef = ref()
//发布
const postUpdate = (row) => {
  updatePostRef.value.open(row)
}
</script>

<style lang="scss" scoped></style>

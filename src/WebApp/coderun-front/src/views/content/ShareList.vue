<template>
  <div class="top-panel">
    <el-card>
      <el-form :model="searchForm" label-width="70px" label-position="right">
        <el-row>
          <el-col :span="4">
            <el-form-item label="标题" label-width="40px">
              <el-input
                class="password-input"
                v-model="searchForm.title"
                clearable
                placeholder="请输入标题"
                @keyup.enter="loadDataList"
              ></el-input>
            </el-form-item>
          </el-col>

          <el-col :span="3">
            <el-form-item label="状态" label-width="50px">
              <el-select v-model="searchForm.status" placeholder="请选择状态" clearable>
                <el-option label="待发布" :value="0"></el-option>
                <el-option label="已发布" :value="1"></el-option>
              </el-select>
            </el-form-item>
          </el-col>

          <el-col :span="4">
            <el-form-item label="创建人">
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
              <el-button type="primary" @click="addShare" v-has="proxy.PermissionCodes.share.edit"
                >新增文章</el-button
              >
              <el-button
                type="primary"
                @click="batchPostShare"
                v-has="proxy.PermissionCodes.share.post"
                :disabled="selectRowData.length == 0"
                >批量发布</el-button
              >
              <el-button
                type="danger"
                @click="batchDelShare"
                v-has="proxy.PermissionCodes.share.del"
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
      <span>经验列表</span>
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
      <template #coverSlot="{ index, row }">
        <Cover
          v-if="row.coverType == 0"
          :width="100"
          :height="100"
          bgColor="#ddd"
          title="无封面"
        ></Cover>
        <Cover v-if="row.coverType == 1" :width="200" :height="100" :cover="row.coverPath"></Cover>
        <Cover v-if="row.coverType == 2" :width="100" :height="100" :cover="row.coverPath"></Cover>
      </template>

      <template #titleSlot="{ index, row }">
        <a class="a-link" @click="showDetailHandler(row)">{{ row.title }}</a>
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
            @click.prevent="editShare(row)"
            v-has="proxy.PermissionCodes.share.edit"
            v-if="row.status == 0"
            >修改</a
          >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="delShare(row)"
            v-has="proxy.PermissionCodes.share.del"
            v-if="
              row.status == 0 &&
              (row.createdUserId == userInfo.account.userId || userInfo.account.isAdmin)
            "
            >删除</a
          >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="postShare(row)"
            v-has="proxy.PermissionCodes.share.post"
            v-if="row.status == 0"
            >发布</a
          >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="cancelShare(row)"
            v-has="proxy.PermissionCodes.question.post"
            v-if="row.status == 1"
            >取消发布</a
          >
        </div>
      </template>
    </Table>
  </el-card>
  <ShareEdit ref="shareEditRef" @reload="loadDataList"></ShareEdit>
  <ShowDetail ref="showDetailRef" :showType="3"></ShowDetail>
</template>

<script setup>
import ShareEdit from './ShareEdit.vue'
import Badge from '@/components/Badge.vue'
import dayjs from 'dayjs'
import ShowDetail from '@/components/content/ShowDetail.vue'
import { ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const userInfo = ref(JSON.parse(sessionStorage.getItem('userInfo')))

const api = {
  LoadShareInfoList: '/ShareInfo/LoadShareInfoList',
  DeletedShareInfo: '/ShareInfo/DeletedShareInfo',
  BatchDeletedShareInfo: '/ShareInfo/BatchDeletedShareInfo',
  PostShareInfo: '/ShareInfo/PostShareInfo',
  CancelShareInfo: '/ShareInfo/CancelShareInfo',
  ShareInfoById: '/ShareInfo/ShareInfoById/',
}

const searchForm = ref({})
const tableData = ref({
  pageIndex: 1,
  pageSize: 30,
})
const tableDataRef = ref()
const tableOptions = ref({
  selectType: 'checkbox',
  extHeight: 120,
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
    url: api.LoadShareInfoList,
    method: 'get',
    dataType: 'json',
    params: params,
  })
  if (!result) return

  tableData.value = result.result
}

const columns = [
  {
    label: '封面',
    prop: 'cover',
    scopedSlots: 'coverSlot',
  },
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

const shareEditRef = ref()
//新增
const addShare = () => {
  shareEditRef.value.open({ shareId: 0 })
}
//修改
const editShare = async (data) => {
  let result = await proxy.Request({
    url: api.ShareInfoById + data.shareId,
    dataType: 'json',
    method: 'get',
  })
  if (!result) return
  shareEditRef.value.open(Object.assign({}, result.result))
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
    selectRowData.value.push(element.shareId)
  })
}
//删除
const delShare = (row) => {
  proxy.Confirm('确定删除这条数据吗', async () => {
    let result = await proxy.Request({
      url: api.DeletedShareInfo,
      dataType: 'json',
      method: 'post',
      params: row.shareId,
    })
    if (!result) return
    proxy.Message.success('删除成功')
    loadDataList()
  })
}
//批量删除
const batchDelShare = () => {
  if (selectRowData.value.length == 0) {
    proxy.Message.warn('请选择删除的数据')
    return
  }
  proxy.Confirm('确定删除选中的数据吗?', async () => {
    let ids = selectRowData.value.join(',')
    let result = await proxy.Request({
      url: api.BatchDeletedShareInfo,
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
const batchPostShare = async () => {
  if (selectRowData.value.length == 0) {
    proxy.Message.warn('请选择发布的数据')
    return
  }
  let ids = selectRowData.value.join(',')
  await post(ids)
}
const postShare = async (row) => {
  await post(row.shareId)
}
const post = async (ids) => {
  let result = await proxy.Request({
    url: api.PostShareInfo,
    dataType: 'json',
    method: 'post',
    params: ids,
  })
  if (!result) return
  proxy.Message.success('发布成功')
  loadDataList()
}
//取消发布
const cancelShare = async (row) => {
  let result = await proxy.Request({
    url: api.CancelShareInfo,
    dataType: 'json',
    method: 'post',
    params: row.shareId,
  })
  if (!result) return
  proxy.Message.success('取消发布成功')
  loadDataList()
}

//显示详情
const showDetailRef = ref()
const showDetailHandler = (row) => {
  showDetailRef.value.open(row.shareId, searchForm.value)
}
</script>

<style lang="scss" scoped></style>

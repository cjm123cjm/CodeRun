<template>
  <div class="top-panel">
    <el-card>
      <el-button type="primary" @click="addAccount" v-has="proxy.PermissionCodes.category.edit"
        >新增分类</el-button
      >
    </el-card>
  </div>
  <el-card class="table-data-card">
    <template #header>
      <span>分类列表</span>
    </template>
    <Table
      ref="tableDataRef"
      :columns="columns"
      :fetch="loadDataList"
      :dataSource="tableData"
      :options="tableOptions"
      :extHeight="tableOptions.extHeight"
      :showPagination="false"
    >
      <template #iconPathSlot="{ index, row }">
        <Cover :cover="row.iconPath" :bgColor="row.bgColor" :title="row.categoryName"></Cover>
      </template>
      <template #typeSlot="{ index, row }">
        <span v-if="row.type == 0">问题分类</span>
        <span v-else-if="row.type == 1">考题分类</span>
        <span v-else>问题/考题分类</span>
      </template>
      <template #operation="{ index, row }">
        <div class="row-op-panel">
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="editAccount(row)"
            v-has="proxy.PermissionCodes.category.edit"
            >修改</a
          >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="delCategory(row)"
            v-has="proxy.PermissionCodes.account.del"
            >删除</a
          >
          <a
            href="javascript:void(0)"
            @click.prevent="changeSort(index, 'up')"
            v-has="proxy.PermissionCodes.category.edit"
            :class="[index == 0 ? 'not-allow' : 'a-link']"
            >上移</a
          >
          <a
            href="javascript:void(0)"
            @click.prevent="changeSort(index, 'down')"
            v-has="proxy.PermissionCodes.category.edit"
            :class="[index == tableData.data.length - 1 ? 'not-allow' : 'a-link']"
            >下移</a
          >
        </div>
      </template>
    </Table>
  </el-card>
  <CategoryEdit ref="categoryEditRef" @reload="loadDataList"></CategoryEdit>
</template>

<script setup>
import CategoryEdit from './CategoryEdit.vue'
import Cover from '@/components/Cover.vue'
import { ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const userInfo = ref(JSON.parse(sessionStorage.getItem('userInfo')))

const api = {
  LoadCategoryList: '/Category/LoadCategoryList',
  SaveCategory: '/Category/SaveCategory',
  DeleteCategory: '/Category/DeleteCategory',
  ChangeCategorySort: '/Category/ChangeCategorySort',
}
const tableData = ref({
  data: [],
})
const tableDataRef = ref()
const tableOptions = ref({
  extHeight: 125,
})

//加载用户列表
const loadDataList = async () => {
  const result = await proxy.Request({
    url: api.LoadCategoryList,
    method: 'get',
    dataType: 'json',
  })
  if (!result) return

  tableData.value.data = result.result
}

const columns = [
  {
    label: '封面',
    prop: 'iconPath',
    scopedSlots: 'iconPathSlot',
  },
  {
    label: '分类名称',
    prop: 'categoryName',
  },
  {
    label: '类型',
    prop: 'type',
    scopedSlots: 'typeSlot',
  },
  {
    label: '操作',
    width: '200',
    scopedSlots: 'operation',
  },
]

//删除
const delCategory = (data) => {
  proxy.Confirm(`确定要删除【${data.categoryName}】分类吗?`, async () => {
    let result = await proxy.Request({
      url: api.DeleteCategory,
      dataType: 'json',
      method: 'post',
      params: data.categoryId,
    })
    if (!result) return
    proxy.Message.success('删除成功')
    loadDataList()
  })
}

//上移下移
const changeSort = async (index, type) => {
  let dataList = tableData.value.data
  if ((type == 'up' && index == 0) || (index == 'down' && index == dataList.length - 1)) {
    return
  }
  let temp = dataList[index]
  let number = type == 'down' ? 1 : -1

  dataList.splice(index, 1)

  dataList.splice(index + number, 0, temp)

  let categoryIds = []
  dataList.forEach((element) => {
    categoryIds.push(element.categoryId)
  })

  let result = await proxy.Request({
    url: api.ChangeCategorySort,
    dataType: 'json',
    method: 'post',
    params: categoryIds.join(','),
  })
  if (!result) return

  loadDataList()
}

const categoryEditRef = ref()
//添加
const addAccount = () => {
  categoryEditRef.value.open({ categoryId: 0 })
}
//修改
const editAccount = (data) => {
  categoryEditRef.value.open(data)
}
</script>

<style lang="scss" scoped></style>

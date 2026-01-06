<template>
  <div class="top-panel">
    <el-card>
      <el-button type="primary" @click="addCarousel" v-has="proxy.PermissionCodes.carousel.edit"
        >新增轮播图</el-button
      >
    </el-card>
  </div>
  <el-card class="table-data-card">
    <template #header>
      <span>轮播图列表</span>
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
      <template #imgPathSlot="{ index, row }">
        <Cover :cover="row.imgPath" :width="330" :height="180"></Cover>
      </template>

      <template #objectTypeSlot="{ index, row }">
        {{ ObjectType[row.objectType] }}
      </template>

      <template #objectSlot="{ index, row }">
        <div v-if="row.objectType == 3">{{ row.outerLink }}</div>
        <div v-else>{{ row.objectId }}</div>
      </template>
      <template #operation="{ index, row }">
        <div class="row-op-panel">
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="editCarousel(row)"
            v-has="proxy.PermissionCodes.carousel.edit"
            >修改</a
          >
          <a
            class="a-link"
            href="javascript:void(0)"
            @click.prevent="delCarousel(row)"
            v-has="proxy.PermissionCodes.carousel.edit"
            >删除</a
          >
          <a
            href="javascript:void(0)"
            @click.prevent="changeSort(index, 'up')"
            v-has="proxy.PermissionCodes.carousel.edit"
            :class="[index == 0 ? 'not-allow' : 'a-link']"
            >上移</a
          >
          <a
            href="javascript:void(0)"
            @click.prevent="changeSort(index, 'down')"
            v-has="proxy.PermissionCodes.carousel.edit"
            :class="[index == tableData.data.length - 1 ? 'not-allow' : 'a-link']"
            >下移</a
          >
        </div>
      </template>
    </Table>
  </el-card>
  <CarouselEdit ref="carouselEditRef" @reload="loadDataList"></CarouselEdit>
</template>

<script setup>
import CarouselEdit from './CarouselEdit.vue'
import Cover from '@/components/Cover.vue'
import { ObjectType } from '@/utils/Constants'
import { ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const api = {
  LoadAppCarouselList: '/AppCarousel/LoadAppCarouselList',
  SaveAppCarousel: '/AppCarousel/SaveAppCarousel',
  DeletedAppCarousel: '/AppCarousel/DeletedAppCarousel',
  ChangeAppCarouselSort: '/AppCarousel/ChangeAppCarouselSort',
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
    url: api.LoadAppCarouselList,
    method: 'get',
    dataType: 'json',
  })
  if (!result) return

  tableData.value.data = result.result
}

const columns = [
  {
    label: '轮播图',
    prop: 'imgPath',
    scopedSlots: 'imgPathSlot',
  },
  {
    label: '类型',
    prop: 'objectType',
    scopedSlots: 'objectTypeSlot',
  },
  {
    label: '主体ID/链接',
    prop: 'object',
    scopedSlots: 'objectSlot',
  },
  {
    label: '操作',
    width: '200',
    scopedSlots: 'operation',
  },
]

//删除
const delCarousel = (data) => {
  proxy.Confirm(`确定要删除轮播图吗?`, async () => {
    let result = await proxy.Request({
      url: api.DeletedAppCarousel,
      dataType: 'json',
      method: 'post',
      params: data.carouselId,
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

  let carouselIds = []
  dataList.forEach((element) => {
    carouselIds.push(element.carouselId)
  })
  debugger

  let result = await proxy.Request({
    url: api.ChangeAppCarouselSort,
    dataType: 'json',
    method: 'post',
    params: carouselIds.join(','),
  })
  if (!result) return

  loadDataList()
}

const carouselEditRef = ref()
//添加
const addCarousel = () => {
  carouselEditRef.value.open({ carouselId: 0 })
}
//修改
const editCarousel = (data) => {
  carouselEditRef.value.open(data)
}
</script>

<style lang="scss" scoped></style>

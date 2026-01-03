<template>
  <div>
    <Dialog
      :title="dialogConfig.title"
      :show="dialogConfig.show"
      :showClose="dialogConfig.showClose"
      :showCancel="dialogConfig.showCancel"
      :buttons="dialogConfig.buttons"
      width="60%"
    >
      <Table
        :dataSource="tableData"
        :showPagination="false"
        :columns="columns"
        :initFetch="false"
        :options="tableOptions"
      >
        <template #errorItemListSlot="{ index, row }">
          <div v-for="(item, index) in row.errorItemList" :key="index">
            {{ index + 1 }}、{{ item }}
          </div>
        </template>
      </Table>
    </Dialog>
  </div>
</template>

<script setup>
import { ref } from 'vue'
const dialogConfig = ref({
  title: '导入错误信息',
  show: false,
  showClose: true,
  showCancel: false,
  buttons: [
    {
      text: '关闭',
      type: 'primary',
      click: () => {
        dialogConfig.value.show = false
      },
    },
  ],
})
const tableData = ref({
  data: [],
})
const tableOptions = ref({})
const columns = [
  {
    label: '错误行',
    prop: 'rowNum',
    width: 100,
  },
  {
    label: '错误原因',
    prop: 'errorItemList',
    scopedSlots: 'errorItemListSlot',
  },
]

const open = (data) => {
  dialogConfig.value.show = true
  tableData.value.data = data
}

defineExpose({
  open,
})
</script>

<style lang="scss" scoped></style>

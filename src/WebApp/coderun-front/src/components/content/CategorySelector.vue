<template>
  <div style="width: 100%">
    <el-select :modelValue="modelValue" placeholder="请选择分类" clearable @change="onChange">
      <el-option
        v-for="category in categories"
        :key="category.categoryId"
        :label="category.categoryName"
        :value="category.categoryId + ''"
      ></el-option>
    </el-select>
  </div>
</template>

<script setup>
import { ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const props = defineProps({
  modelValue: {
    type: [Number, String],
  },
  type: {
    type: Number,
  },
})

const api = {
  LoadCategoryList: '/Category/LoadCategoryList',
}

const categories = ref([])

const loadCategories = async () => {
  const result = await proxy.Request({
    url: api.LoadCategoryList,
    method: 'get',
    dataType: 'json',
  })
  if (!result) return

  categories.value = result.result
}
loadCategories()

const emit = defineEmits(['update:modelValue', 'change'])
const onChange = (value) => {
  emit('update:modelValue', value)
  emit('change')
}
</script>

<style lang="scss" scoped></style>

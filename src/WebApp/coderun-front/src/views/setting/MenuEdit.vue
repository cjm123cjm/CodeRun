<template>
  <Dialog
    :title="dialogConfig.title"
    :show="dialogConfig.show"
    :showClose="dialogConfig.showClose"
    :showCancel="dialogConfig.showCancel"
    :buttons="dialogConfig.buttons"
    width="600px"
    @close="dialogConfig.show = false"
  >
    <el-form label-width="80px" :model="formData" ref="formDataRef" :rules="rules">
      <el-form-item label="菜单名称" prop="menuName">
        <el-input v-model="formData.menuName" clearable placeholder="请输入菜单名称" />
      </el-form-item>
      <el-form-item label="父级菜单" prop="parentId" v-if="formData.parentId != 0">
        <el-tree-select
          v-model="formData.parentId"
          :data="treeData"
          :props="treeProps"
          default-expand-all
          node-key="menuId"
          show-search
          placeholder="请选择父级菜单"
          clearable
          :check-strictly="true"
          style="width: 100%"
        >
        </el-tree-select>
      </el-form-item>
      <el-form-item label="菜单类型">
        <el-radio-group v-model="formData.menuType">
          <el-radio :label="0">菜单</el-radio>
          <el-radio :label="1">按钮</el-radio>
        </el-radio-group>
      </el-form-item>
      <el-form-item label="菜单路径" prop="menuUrl" v-if="formData.menuType == 0">
        <el-input
          v-model="formData.menuUrl"
          clearable
          placeholder="请输入菜单路径"
          :maxLength="50"
        />
      </el-form-item>
      <el-form-item label="权限编码" prop="permissionCode">
        <el-input v-model="formData.permissionCode" :maxLength="50" clearable />
      </el-form-item>
      <el-form-item label="菜单图标" prop="icon">
        <el-input v-model="formData.icon" :maxLength="10" clearable placeholder="eg:home" />
      </el-form-item>
      <el-form-item label="排序" prop="sort">
        <el-input v-model="formData.sort" :maxLength="10" clearable />
      </el-form-item>
    </el-form>
  </Dialog>
</template>

<script setup>
import { nextTick, ref, getCurrentInstance } from 'vue'
const { proxy } = getCurrentInstance()

const props = defineProps({
  treeData: {
    type: Array,
    default: [],
  },
})
const emits = defineEmits('reload')
const treeProps = {
  class: 'custom-tree-node',
  children: 'childMenu',
  label: 'menuName',
  value: 'menuId',
}
const api = {
  saveMenu: '/Menu/SaveMenu',
}

const dialogConfig = ref({
  title: '编辑菜单',
  show: false,
  showClose: true,
  showCancel: true,
  buttons: [
    {
      text: '确定',
      type: 'primary',
      click: () => {
        submitForm()
      },
    },
  ],
})

//数据
const formData = ref({})
const formDataRef = ref(null)
const rules = {
  menuName: [{ required: true, message: '菜单名称不能为空', trigger: 'blur' }],
  parentId: [{ required: true, message: '请选择父级菜单', trigger: 'blur' }],
  menuType: [{ required: true, message: '请选择菜单类型', trigger: 'blur' }],
  menuUrl: [{ required: true, message: '请输入菜单路径', trigger: 'blur' }],
  permissionCode: [{ required: true, message: '请输入权限编码', trigger: 'blur' }],
  sort: [
    { required: true, message: '请输入排序号', trigger: 'blur' },
    {
      type: proxy.Verify.number,
      message: '排序号必须为数字值',
      trigger: 'blur',
    },
  ],
}

//显示
const show = (type, data) => {
  dialogConfig.value.show = true
  nextTick(() => {
    formDataRef.value.resetFields()
    if (type == 'edit') {
      dialogConfig.value.title = '编辑菜单'
      formData.value = Object.assign({}, data)
    } else {
      dialogConfig.value.title = '添加菜单'
      formData.value = {
        parentId: data.menuId,
      }
    }
  })
}

//保存
const submitForm = () => {
  formDataRef.value.validate(async (valid) => {
    if (!valid) return
    let params = {}
    Object.assign(params, formData.value)
    delete params.children
    let result = await proxy.Request({
      url: api.saveMenu,
      dataType: 'json',
      method: 'post',
      params: params,
    })
    if (!result) return

    dialogConfig.value.show = false
    proxy.Message.success('保存成功')
    emits('reload')
  })
}

defineExpose({
  show,
})
</script>

<style lang="scss" scoped></style>
